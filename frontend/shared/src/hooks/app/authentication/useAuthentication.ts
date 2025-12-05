import { useAtom, useAtomValue } from "jotai";
import { useQueryClient } from "@tanstack/react-query";
import { useCallback } from "react";
import {
  TokenRequest,
  useTokenMutation
} from "../../queries/authentication/useTokenMutation";
import { appConfigAtom } from "../../../state/appConfig";
import { add, isAfter, parseISO, sub } from "date-fns";
import { randomInteger } from "../../../utils/numbers";
import { LoginFormValues } from "../../user/login/LoginFormValues";
import { useResetCurrent } from "../../current/useResetCurrent";
import { atomWithStorage, createJSONStorage } from "jotai/utils";
import { getFromAsyncStorage } from "../../../utils/asyncStorage";
import AsyncStorage from "@react-native-async-storage/async-storage";

export type ExternalAuthenticationProvider = "apple" | "microsoft" | "google";

type Token = {
  accessToken: string;
  refreshToken: string;
  expiryDate: string;
  date: string;
};

type RefreshLock = {
  date: string;
};

let localRefreshLock: RefreshLock | undefined = undefined;

async function clearRefreshLock(force?: boolean) {
  if (
    force ||
    (localRefreshLock !== undefined &&
      isAfter(new Date(), add(parseISO(localRefreshLock.date), { seconds: 2 })))
  ) {
    localRefreshLock = undefined;
  }
}

function tokenIsExpired(expiryDate: string): boolean {
  const expiryDateAdjusted = sub(parseISO(expiryDate), {
    seconds: randomInteger(120, 300)
  });

  const expired = isAfter(new Date(), expiryDateAdjusted);

  return expired;
}

type AuthenticationState = {
  initialized: boolean;
  error?: string;
  token?: Token;
  external?: {
    provider: ExternalAuthenticationProvider;
    token?: string;
  };
};

const storageKey = "Navtrack:Authentication";

const authenticationAtom = atomWithStorage<AuthenticationState>(
  storageKey,
  {
    initialized: false
  },
  createJSONStorage<AuthenticationState>(() => AsyncStorage),
  { getOnInit: true }
);

export function useAuthentication() {
  const appConfig = useAtomValue(appConfigAtom);
  const queryClient = useQueryClient();
  const resetCurrent = useResetCurrent();
  const [state, setState] = useAtom(authenticationAtom);

  const tokenMutation = useTokenMutation({
    options: {
      onMutate: () => {
        setState(async (prev) => {
          const newState: AuthenticationState = {
            ...(await prev),
            error: undefined
          };
          return newState;
        });
      },
      onSuccess: async (data) => {
        const token: Token = {
          accessToken: data.access_token,
          refreshToken: data.refresh_token,
          expiryDate: add(new Date(), {
            seconds: data.expires_in
          }).toISOString(),
          date: new Date().toISOString()
        };

        setState(async (prev) => {
          const newState: AuthenticationState = {
            ...(await prev),
            error: undefined,
            external: undefined,
            token
          };

          return newState;
        });
      },
      onError: (error, data) => {
        const accountNotLinkedError =
          error.response?.data.code === "LOGIN_000002";

        resetCurrent();

        setState(async (prev) => {
          const p = await prev;
          const newState: AuthenticationState = {
            ...p,
            token: undefined,
            error: accountNotLinkedError
              ? undefined
              : error.response?.data.code,
            external:
              p.external !== undefined
                ? p.external
                : accountNotLinkedError
                  ? {
                      provider:
                        data.grant_type as ExternalAuthenticationProvider,
                      token: error.response?.data.token ?? data.code
                    }
                  : undefined
          };

          return newState;
        });
      },
      onSettled: () => {
        setState((prev) => ({
          ...prev,
          isLoading: false
        }));
      }
    }
  });

  const getAccessToken = useCallback(async () => {
    await clearRefreshLock();

    const storageState =
      await getFromAsyncStorage<AuthenticationState>(storageKey);

    if (
      !localRefreshLock &&
      !!storageState?.token &&
      tokenIsExpired(storageState.token.expiryDate)
    ) {
      try {
        localRefreshLock = {
          date: new Date().toISOString()
        };

        const data = {
          grant_type: "refresh_token",
          client_id: appConfig?.authentication?.clientId!,
          refresh_token: storageState.token.refreshToken
        };

        const response = await tokenMutation.mutateAsync(data);

        return response.access_token;
      } finally {
        await clearRefreshLock(true);
      }
    }

    return storageState?.token?.accessToken;
  }, [appConfig?.authentication?.clientId, tokenMutation]);

  const internalLogin = useCallback(
    (values: LoginFormValues) => {
      const data = {
        grant_type: "password",
        username: values.email,
        password: values.password,
        scope: "offline_access IdentityServerApi openid",
        client_id: appConfig?.authentication?.clientId!
      };

      tokenMutation.mutate(data);
    },
    [appConfig?.authentication?.clientId, tokenMutation]
  );

  const externalLogin = useCallback(
    (
      token: string,
      provider: ExternalAuthenticationProvider,
      password: string | undefined = undefined
    ) => {
      const data: TokenRequest = {
        grant_type: provider,
        code: token,
        password: password,
        scope: "offline_access IdentityServerApi openid",
        client_id: appConfig?.authentication.clientId!
      };

      tokenMutation.mutate(data);
    },
    [appConfig?.authentication.clientId, tokenMutation]
  );

  const logout = useCallback(() => {
    setState({
      initialized: true,
      token: undefined,
      error: undefined,
      external: undefined
    });
    queryClient.clear();
    resetCurrent();
  }, [queryClient, resetCurrent, setState]);

  const initialize = useCallback(async () => {
    getAccessToken().finally(() => {
      setState((prev) => ({
        ...prev,
        initialized: true
      }));
    });
  }, [getAccessToken, setState]);

  const clearErrors = useCallback(
    () =>
      setState((prev) => ({
        ...prev,
        error: undefined
      })),
    [setState]
  );

  const clearExternal = useCallback(
    () =>
      setState((prev) => ({
        ...prev,
        error: undefined,
        external: undefined
      })),
    [setState]
  );

  return {
    isAuthenticated: !!state.token,
    isLoading: tokenMutation.isPending,
    initialize,
    getAccessToken,
    internalLogin,
    externalLogin,
    logout,
    clearErrors,
    clearExternal,
    state
  };
}
