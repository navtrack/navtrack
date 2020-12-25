import { useContext, useEffect } from "react";
import { LoginModel } from "../../components/login/LoginModel";
import { ApiError } from "../ApiError";
import { AppContext } from "../appContext/AppContext";
import { useConnectTokenMutation } from "../mutations/useConnectTokenMutation";

type IUseLogin = {
  login: (model: LoginModel) => void;
  isLoading: boolean;
  isSuccess: boolean;
  error?: ApiError;
};

export const useLogin = (): IUseLogin => {
  const { appContext, setAppContext } = useContext(AppContext);
  const mutation = useConnectTokenMutation();

  useEffect(() => {
    if (mutation.isSuccess && mutation.data) {
      setAppContext({
        ...appContext,
        authenticationInfo: {
          access_token: mutation.data.access_token,
          refresh_token: mutation.data.refresh_token,
          authenticated: true,
          email: "",
          expiry_date: new Date(new Date().getTime() + mutation.data.expires_in * 1000).toString(),
          session_expired: false
        }
      });
    }
  }, [appContext, mutation, setAppContext]);

  const login = (model: LoginModel) => {
    mutation.mutate({
      grant_type: "password",
      username: model.email,
      password: model.password,
      scope: "offline_access IdentityServerApi openid",
      client_id: "navtrack.web"
    });
  };

  return {
    login,
    isLoading: mutation.isLoading,
    isSuccess: mutation.isSuccess,
    error: mutation.isError ? { message: mutation.error?.error_description } : undefined
  };
};
