import { useCallback } from "react";
import { useHistory } from "react-router";
import { useGetTokenMutation } from "../mutations/useGetTokenMutation";
import { LoginFormValues } from "../../components/login/LoginFormValues";
import useAppContext from "../app/useAppContext";
import { AUTHENTICATION } from "../../constants";

export const useLogin = () => {
  const { setAppContext } = useAppContext();
  const history = useHistory();

  const getTokenMutation = useGetTokenMutation({
    onSuccess: (data) => {
      setAppContext((current) => ({
        ...current,
        isAuthenticated: true,
        token: {
          accessToken: data.access_token,
          refreshToken: data.refresh_token,
          expiryDate: new Date(new Date().getTime() + data.expires_in * 1000)
        }
      }));

      history.push("/");
    }
  });

  const login = useCallback(
    (values: LoginFormValues) => {
      const data = {
        grant_type: "password",
        username: values.email,
        password: values.password,
        scope: "offline_access IdentityServerApi openid",
        client_id: AUTHENTICATION.CLIENT_ID
      };

      getTokenMutation.mutate(data);
    },
    [getTokenMutation]
  );

  return { login, loading: getTokenMutation.isLoading };
};
