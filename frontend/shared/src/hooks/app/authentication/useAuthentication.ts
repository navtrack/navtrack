import { atom, useRecoilState, useRecoilValue } from "recoil";
import { useQueryClient } from "@tanstack/react-query";
import { useCallback } from "react";
import {
  TokenRequest,
  useTokenMutation
} from "../../mutations/authentication/useTokenMutation";
import { appConfigAtom } from "../../../state/appConfig";
import { add, isAfter, parseISO, sub } from "date-fns";
import { randomInteger } from "../../../utils/numbers";
import {
  getFromAsyncStorage,
  setInAsyncStorage,
  removeFromAsyncStorage
} from "../../../utils/asyncStorage";

export type ExternalAuthenticationProvider = "apple" | "microsoft" | "google";

export type LoginValues = {
  username: string;
  password: string;
};

export type Token = {
  accessToken: string;
  refreshToken: string;
  expiryDate: string;
  date: string;
};

export type AuthenticationStorageState = {
  token?: Token;
};

const authenticationStorageKey = "Navtrack:Authentication";

export const AuthenticationStorage = {
  get: () =>
    getFromAsyncStorage<AuthenticationStorageState>(authenticationStorageKey),
  set: (value: AuthenticationStorageState) =>
    setInAsyncStorage(authenticationStorageKey, value),
  clear: () => removeFromAsyncStorage(authenticationStorageKey)
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

function getExpiryDate(expiresIn: number) {
  const date = add(new Date(), {
    seconds: expiresIn
  }).toISOString();

  return date;
}

type AuthenticationState = {
  isAuthenticated: boolean;
  initialized: boolean;
  error?: string;
  isLoading: boolean;
  external?: {
    error?: string;
    provider: ExternalAuthenticationProvider;
    token?: string;
  };
};

async function authenticationDefault() {
  const state = await AuthenticationStorage.get();

  const defaultState: AuthenticationState = {
    isAuthenticated: state?.token !== undefined,
    initialized: false,
    isLoading: false
  };

  return defaultState;
}

const authenticationAtom = atom<AuthenticationState>({
  key: "Navtrack:Authentication",
  default: authenticationDefault()
});

export function useAuthentication() {
  const appConfig = useRecoilValue(appConfigAtom);
  const queryClient = useQueryClient();
  const [state, setState] = useRecoilState(authenticationAtom);

  const tokenMutation = useTokenMutation({
    options: {
      onMutate: () => {
        setState((prev) => ({
          ...prev,
          isLoading: true
        }));
      },
      onSuccess: async (data) => {
        const token: Token = {
          accessToken: data.access_token,
          refreshToken: data.refresh_token,
          expiryDate: getExpiryDate(data.expires_in),
          date: new Date().toISOString()
        };

        AuthenticationStorage.set({ token });
        setState((prev) => ({
          ...prev,
          isAuthenticated: true,
          error: undefined
        }));
      },
      onError: (error, data) => {
        const accountNotLinkedError = error.response?.data.code === "100022";

        AuthenticationStorage.clear();
        setState((prev) => ({
          ...prev,
          isAuthenticated: false,
          error: accountNotLinkedError ? undefined : error.response?.data.code,
          external:
            prev.external !== undefined
              ? prev.external
              : accountNotLinkedError
              ? {
                  provider: data.grant_type as ExternalAuthenticationProvider,
                  token: error.response?.data.token ?? data.code,
                  error: error.response?.data.code
                }
              : undefined
        }));
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
    const authenticationStorage = await AuthenticationStorage.get();

    if (
      !localRefreshLock &&
      authenticationStorage?.token !== undefined &&
      tokenIsExpired(authenticationStorage.token.expiryDate)
    ) {
      try {
        localRefreshLock = {
          date: new Date().toISOString()
        };

        const data = {
          grant_type: "refresh_token",
          client_id: appConfig?.authentication?.clientId!,
          refresh_token: authenticationStorage.token.refreshToken
        };

        const response = await tokenMutation.mutateAsync(data);

        return response.access_token;
      } finally {
        await clearRefreshLock(true);
      }
    }

    return authenticationStorage?.token?.accessToken;
  }, [appConfig?.authentication?.clientId, tokenMutation]);

  const internalLogin = useCallback(
    (values: LoginValues) => {
      const data = {
        grant_type: "password",
        username: values.username,
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
    AuthenticationStorage.clear();
    setState((prev) => ({
      ...prev,
      isAuthenticated: false,
      token: undefined,
      error: undefined,
      external: undefined
    }));
    queryClient.clear();
  }, [queryClient, setState]);

  const clearErrors = useCallback(
    (includeExternal: boolean = false) => {
      setState((prev) => ({
        ...prev,
        error: undefined,
        external: includeExternal ? undefined : prev.external
      }));
    },
    [setState]
  );

  const initialize = useCallback(async () => {
    getAccessToken().finally(() => {
      setState((prev) => ({
        ...prev,
        initialized: true
      }));
    });
  }, [getAccessToken, setState]);

  return {
    initialize,
    getAccessToken,
    internalLogin,
    externalLogin,
    logout,
    clearErrors,
    state
  };
}
