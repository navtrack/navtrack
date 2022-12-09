import { useCallback, useState } from "react";
import { useGetTokenMutation } from "../mutations/useGetTokenMutation";
import { useSetRecoilState } from "recoil";
import { add } from "date-fns";
import { authenticationAtom } from "../../state/authentication";

export type LoginValues = {
  username: string;
  password: string;
};

interface UseLoginProps {
  clientId: string;
  onSuccess?: () => void;
}

export const useLogin = (props: UseLoginProps) => {
  const setState = useSetRecoilState(authenticationAtom);
  const [internalLoginError, setInternalLoginError] = useState(false);
  const [externalLoginError, setExternalLoginError] = useState(false);

  const getTokenMutation = useGetTokenMutation({
    onSuccess: (data, variables) => {
      setState(() => ({
        recheckToken: false,
        initialized: true,
        isAuthenticated: true,
        email: variables.username,
        token: {
          accessToken: data.access_token,
          refreshToken: data.refresh_token,
          expiryDate: add(new Date(), {
            seconds: data.expires_in
          }).toISOString()
        }
      }));

      props.onSuccess?.();
    },
    onError: (_, data) => {
      if (data.grant_type === "password") {
        setInternalLoginError(true);
      } else {
        setExternalLoginError(true);
      }
    }
  });

  const internalLogin = useCallback(
    (values: LoginValues) => {
      const data = {
        grant_type: "password",
        username: values.username,
        password: values.password,
        scope: "offline_access IdentityServerApi openid",
        client_id: props.clientId
      };

      setInternalLoginError(false);
      setExternalLoginError(false);
      getTokenMutation.mutate(data);
    },
    [getTokenMutation, props.clientId]
  );

  const externalLogin = useCallback(
    (code: string, grantType: "apple" | "microsoft" | "google") => {
      const data = {
        grant_type: grantType,
        code: code,
        scope: "offline_access IdentityServerApi openid",
        client_id: props.clientId
      };

      setInternalLoginError(false);
      setExternalLoginError(false);
      getTokenMutation.mutate(data);
    },
    [getTokenMutation, props.clientId]
  );

  return {
    internalLogin,
    externalLogin,
    loading: getTokenMutation.isLoading,
    internalLoginError,
    externalLoginError
  };
};
