import { atom, useAtomValue } from "jotai";
import { useQueryClient } from "@tanstack/react-query";
import { useCallback } from "react";
import {
  TokenError,
  TokenRequest,
  TokenResponse,
  useTokenMutation
} from "../../queries/authentication/useTokenMutation";
import { isAxiosError } from "axios";
import { add, isAfter, parseISO, sub } from "date-fns";
import { randomInteger } from "../../../utils/numbers";
import { useLogoutMutation } from "../../queries/authentication/useLogoutMutation";
import { LoginFormValues } from "../../user/login/LoginFormValues";
import { atomWithStorage, createJSONStorage } from "jotai/vanilla/utils";
import { appConfigStore } from "../../../state/appConfig";
import { jotaiStore } from "../../../state/store";
import { AsyncLocalStorage } from "../../../utils/asyncLocalStorage";
import { IS_WEB } from "../../../utils/web";
import AsyncStorage from "@react-native-async-storage/async-storage";

export type ExternalAuthenticationProvider = "apple" | "microsoft" | "google";

type Token = {
  accessToken: string;
  refreshToken: string;
  expiryDate: string;
  date: string;
};

type AuthenticationState = {
  // Changes on login/logout, so another tab cannot save a response from an old session.
  sessionId?: string;
  error?: string;
  token?: Token;
  external?: {
    provider: ExternalAuthenticationProvider;
    token?: string;
  };
};

const authenticationStorageKey = "Navtrack:Authentication:OpenIddict";
const authenticationStorage = IS_WEB
  ? createJSONStorage<AuthenticationState>(() => AsyncLocalStorage)
  : createJSONStorage<AuthenticationState>(() => AsyncStorage);
const authenticationAtom = atomWithStorage<AuthenticationState>(
  authenticationStorageKey,
  {},
  authenticationStorage,
  { getOnInit: true }
);

type Operation = "signIn" | "refresh" | "signOut";
const operationsAtom = atom<Record<Operation, number | undefined>>({
  signIn: undefined,
  refresh: undefined,
  signOut: undefined
});
let operationId = 0;
let sessionGeneration = 0;
let stateQueue: Promise<unknown> = Promise.resolve();
let refreshRequest:
  | { generation: number; promise: Promise<string | undefined> }
  | undefined;

function startOperation(operation: Operation): number {
  const id = ++operationId;
  jotaiStore.set(operationsAtom, (previous) => ({
    ...(operation === "refresh"
      ? previous
      : { signIn: undefined, refresh: undefined, signOut: undefined }),
    [operation]: id
  }));
  return id;
}

function finishOperation(operation: Operation, id: number) {
  jotaiStore.set(operationsAtom, (previous) =>
    previous[operation] === id
      ? { ...previous, [operation]: undefined }
      : previous
  );
}

function withStateLock<T>(action: () => Promise<T>): Promise<T> {
  // Serialize writes on mobile too, where persistence is asynchronous.
  const run = async () =>
    IS_WEB && typeof navigator !== "undefined" && navigator.locks
      ? navigator.locks.request("Navtrack:Authentication", action)
      : action();
  const result = stateQueue.then(run, run);
  stateQueue = result.catch(() => undefined);
  return result;
}

async function readState(): Promise<AuthenticationState> {
  return IS_WEB
    ? authenticationStorage.getItem(authenticationStorageKey, {})
    : jotaiStore.get(authenticationAtom);
}

function updateState(
  update: (previous: AuthenticationState) => AuthenticationState | undefined
): Promise<AuthenticationState | undefined> {
  return withStateLock(async () => {
    const previous = await readState();
    const next = update(previous);
    if (next !== undefined) await jotaiStore.set(authenticationAtom, next);
    return next;
  });
}

function newSessionId(): string {
  // This is a concurrency marker, not a credential; React Native may not expose crypto.
  return globalThis.crypto?.randomUUID?.() ?? `${Date.now()}-${Math.random()}`;
}

function toToken(data: TokenResponse): Token {
  return {
    accessToken: data.access_token,
    refreshToken: data.refresh_token,
    expiryDate: add(new Date(), { seconds: data.expires_in }).toISOString(),
    date: new Date().toISOString()
  };
}

function tokenIsExpired(expiryDate: string): boolean {
  return isAfter(
    new Date(),
    sub(parseISO(expiryDate), { seconds: randomInteger(120, 300) })
  );
}

type AuthenticationProps = {
  onLogin?: () => void;
  onLogout?: () => void;
};

export function useAuthentication(props?: AuthenticationProps) {
  const queryClient = useQueryClient();
  const state = useAtomValue(authenticationAtom);
  const operations = useAtomValue(operationsAtom);
  const tokenMutation = useTokenMutation({});
  const logoutMutation = useLogoutMutation({});

  const getAccessToken = useCallback(async () => {
    const generation = sessionGeneration;
    if (refreshRequest?.generation === generation)
      return refreshRequest.promise;

    const refresh = async () => {
      // Another tab may have rotated the token while this request waited for its lock.
      const snapshot = await withStateLock(readState);
      if (generation !== sessionGeneration) return undefined;
      const token = snapshot.token;
      if (!token || !tokenIsExpired(token.expiryDate))
        return token?.accessToken;

      const id = startOperation("refresh");
      const isCurrent = (current: AuthenticationState) =>
        generation === sessionGeneration &&
        current.sessionId === snapshot.sessionId &&
        current.token?.refreshToken === token.refreshToken;
      try {
        const response = await tokenMutation.mutateAsync({
          grant_type: "refresh_token",
          client_id: appConfigStore.config.authentication.clientId,
          refresh_token: token.refreshToken
        });
        const updated = await updateState((current) =>
          isCurrent(current)
            ? { ...current, token: toToken(response) }
            : undefined
        );
        return generation === sessionGeneration
          ? updated?.token?.accessToken
          : undefined;
      } catch (error) {
        // Only a rejected refresh credential ends the session. Offline/5xx failures do not.
        if (
          isAxiosError<TokenError>(error) &&
          error.response?.status === 400 &&
          error.response.data?.error === "invalid_grant"
        ) {
          const cleared = await updateState((current) =>
            isCurrent(current) ? { sessionId: current.sessionId } : undefined
          );
          if (cleared && generation === sessionGeneration) queryClient.clear();
        }
        throw error;
      } finally {
        finishOperation("refresh", id);
      }
    };

    const request = {
      generation,
      promise: Promise.resolve<string | undefined>(undefined)
    };
    request.promise = Promise.resolve()
      .then(async () =>
        IS_WEB && typeof navigator !== "undefined" && navigator.locks
          ? navigator.locks.request("Navtrack:RefreshToken", refresh)
          : refresh()
      )
      .finally(() => {
        if (refreshRequest === request) refreshRequest = undefined;
      });
    refreshRequest = request;
    return request.promise;
  }, [queryClient, tokenMutation]);

  const signIn = useCallback(
    async (data: TokenRequest): Promise<boolean> => {
      const generation = ++sessionGeneration;
      const sessionId = newSessionId();
      const id = startOperation("signIn");
      let authenticated = false;
      try {
        const started = await updateState((previous) =>
          generation === sessionGeneration
            ? { ...previous, sessionId, token: undefined, error: undefined }
            : undefined
        );
        if (!started) return false;
        const response = await tokenMutation.mutateAsync(data);
        const updated = await updateState((current) =>
          generation === sessionGeneration && current.sessionId === sessionId
            ? { sessionId, token: toToken(response) }
            : undefined
        );
        authenticated = updated !== undefined;
      } catch (error) {
        const tokenError = isAxiosError<TokenError>(error)
          ? error.response?.data
          : undefined;
        const accountNotLinked = tokenError?.code === "LOGIN_000002";
        await updateState((current) =>
          generation === sessionGeneration && current.sessionId === sessionId
            ? {
                ...current,
                error: accountNotLinked ? undefined : tokenError?.code,
                external:
                  current.external ??
                  (accountNotLinked
                    ? {
                        provider:
                          data.grant_type as ExternalAuthenticationProvider,
                        token: tokenError?.token ?? data.code
                      }
                    : undefined)
              }
            : undefined
        );
      } finally {
        finishOperation("signIn", id);
      }
      // Event handlers can ignore the promise; expected login failures resolve to false.
      if (!authenticated || generation !== sessionGeneration) return false;
      props?.onLogin?.();
      return true;
    },
    [props?.onLogin, tokenMutation]
  );

  const internalLogin = useCallback(
    (values: LoginFormValues) =>
      signIn({
        grant_type: "password",
        username: values.email,
        password: values.password,
        scope: "offline_access openid",
        client_id: appConfigStore.config.authentication.clientId
      }),
    [signIn]
  );

  const externalLogin = useCallback(
    (
      token: string,
      provider: ExternalAuthenticationProvider,
      password: string | undefined = undefined
    ) =>
      signIn({
        grant_type: provider,
        code: token,
        password,
        scope: "offline_access openid",
        client_id: appConfigStore.config.authentication.clientId
      }),
    [signIn]
  );

  const logout = useCallback(async () => {
    // Invalidate in-flight work immediately, before waiting for storage or the server.
    const generation = ++sessionGeneration;
    const sessionId = newSessionId();
    const id = startOperation("signOut");
    try {
      const started = await updateState((previous) =>
        generation === sessionGeneration
          ? { ...previous, sessionId }
          : undefined
      );
      if (started && generation === sessionGeneration) {
        const accessToken = await getAccessToken();
        const current = await withStateLock(readState);
        if (
          accessToken &&
          generation === sessionGeneration &&
          current.sessionId === sessionId
        ) {
          // Pin the credential: the Axios auth interceptor could otherwise select a newer login.
          await logoutMutation.mutateAsync(accessToken);
        }
      }
    } catch {
      // An unavailable server must not prevent clearing this device's session.
    } finally {
      try {
        const cleared = await updateState((current) =>
          generation === sessionGeneration && current.sessionId === sessionId
            ? { sessionId }
            : undefined
        );
        if (cleared && generation === sessionGeneration) {
          queryClient.clear();
          props?.onLogout?.();
        }
      } finally {
        finishOperation("signOut", id);
      }
    }
  }, [getAccessToken, logoutMutation, props?.onLogout, queryClient]);

  const clearErrors = useCallback(
    () => updateState((previous) => ({ ...previous, error: undefined })),
    []
  );
  const clearExternal = useCallback(
    () =>
      updateState((previous) => ({
        ...previous,
        error: undefined,
        external: undefined
      })),
    []
  );

  return {
    isAuthenticated: !!state.token,
    isLoading: operations.signIn !== undefined,
    isSigningIn: operations.signIn !== undefined,
    isRefreshing: operations.refresh !== undefined,
    isSigningOut: operations.signOut !== undefined,
    getAccessToken,
    internalLogin,
    externalLogin,
    logout,
    clearErrors,
    clearExternal,
    state
  };
}
