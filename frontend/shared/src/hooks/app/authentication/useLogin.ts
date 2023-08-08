import { useCallback } from "react";
import { useRecoilValue } from "recoil";
import { appConfigAtom } from "../../../state/appConfig";
import { useTokenMutation } from "./useTokenMutation";
import { useAuthentication } from "./useAuthentication";

export type LoginValues = {
  username: string;
  password: string;
};

export function useLogin() {
  const appConfig = useRecoilValue(appConfigAtom);
  const tokenMutation = useTokenMutation();
  const authentication = useAuthentication();

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
    (code: string, provider: "apple" | "microsoft" | "google") => {
      const data = {
        grant_type: provider,
        code: code,
        scope: "offline_access IdentityServerApi openid",
        client_id: appConfig?.authentication.clientId!
      };

      tokenMutation.mutate(data);
    },
    [appConfig?.authentication.clientId, tokenMutation]
  );

  return {
    internalLogin,
    externalLogin,
    logout: () => authentication.clear(),
    loading: tokenMutation.isLoading,
    error: authentication.error
  };
}
