import { useAtom } from "jotai";
import { useQueryClient } from "@tanstack/react-query";
import { useCallback } from "react";
import {
  TokenRequest,
  useTokenMutation
} from "../../queries/authentication/useTokenMutation";
import { add, isAfter, parseISO, sub } from "date-fns";
import { randomInteger } from "../../../utils/numbers";
import { LoginFormValues } from "../../user/login/LoginFormValues";
import { atomWithStorage, createJSONStorage } from "jotai/utils";
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
  error?: string;
  token?: Token;
  external?: {
    provider: ExternalAuthenticationProvider;
    token?: string;
  };
};

const authenticationAtom = atomWithStorage<AuthenticationState>(
  "Navtrack:Authentication",
  {},
  IS_WEB
    ? createJSONStorage(() => AsyncLocalStorage)
    : createJSONStorage(() => AsyncStorage),
  { getOnInit: true }
);

type AuthenticationProps = {
  onLogin?: () => void;
  onLogout?: () => void;
};

export function useAuthentication(props?: AuthenticationProps) {
  const queryClient = useQueryClient();
  const [state, setState] = useAtom(authenticationAtom);

  const tokenMutation = useTokenMutation({
    options: {
      onMutate: () => {
        setState((prev) => {
          const newState: AuthenticationState = {
            ...prev,
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

        setState(async (p) => {
          const prev = await p;
          const newState: AuthenticationState = {
            ...prev,
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

        setState(async (p) => {
          const prev = await p;
          const newState: AuthenticationState = {
            ...prev,
            token: undefined,
            error: accountNotLinkedError
              ? undefined
              : error.response?.data.code,
            external:
              prev.external !== undefined
                ? prev.external
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
        setState(async (prev) => ({
          ...(await prev),
          isLoading: false
        }));
      }
    }
  });

  const getAccessToken = useCallback(async () => {
    await clearRefreshLock();
    const state = await jotaiStore.get(authenticationAtom);

    if (
      !localRefreshLock &&
      !!state?.token &&
      tokenIsExpired(state.token.expiryDate)
    ) {
      try {
        localRefreshLock = {
          date: new Date().toISOString()
        };

        const data = {
          grant_type: "refresh_token",
          client_id: appConfigStore.config?.authentication?.clientId!,
          refresh_token: state.token.refreshToken
        };

        const response = await tokenMutation.mutateAsync(data);

        return response.access_token;
      } finally {
        await clearRefreshLock(true);
      }
    }

    return state?.token?.accessToken;
  }, [tokenMutation]);

  const internalLogin = useCallback(
    (values: LoginFormValues) => {
      const data = {
        grant_type: "password",
        username: values.email,
        password: values.password,
        scope: "offline_access IdentityServerApi openid",
        client_id: appConfigStore.config?.authentication?.clientId!
      };

      tokenMutation.mutate(data);
      props?.onLogin?.();
    },
    [props, tokenMutation]
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
        client_id: appConfigStore.config?.authentication?.clientId!
      };

      tokenMutation.mutate(data);
      props?.onLogin?.();
    },
    [props, tokenMutation]
  );

  const logout = useCallback(() => {
    setState({
      token: undefined,
      error: undefined,
      external: undefined
    });
    queryClient.clear();
    props?.onLogout?.();
  }, [props, queryClient, setState]);

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
    getAccessToken,
    internalLogin,
    externalLogin,
    logout,
    clearErrors,
    clearExternal,
    state
  };
}
