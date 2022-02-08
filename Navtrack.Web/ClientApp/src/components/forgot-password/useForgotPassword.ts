import { useGetTokenMutation } from "@navtrack/navtrack-app-shared";
import { useCallback } from "react";
import { useHistory } from "react-router";
import { ForgotPasswordFormValues } from "./ForgotPasswordFormValues";

export const useForgotPassword = () => {
  // const [user, setUser] = useRecoilState(currentUserAtom);
  const history = useHistory();

  const getTokenMutation = useGetTokenMutation({
    onSuccess: (data) => {
      // setUser((current) => ({
      //   isAuthenticated: true,
      //   user: {
      //     accessToken: data.access_token
      //   }
      // }));
      history.push("/");
    }
  });

  const register = useCallback((values: ForgotPasswordFormValues) => {
    // const data = {
    //   grant_type: "password",
    //   // username: username,
    //   // password: password,
    //   scope: "offline_access IdentityServerApi openid",
    //   client_id: "navtrack.web"
    // };
    // getTokenMutation.mutate(data);
  }, []);

  return {
    register,
    loading: getTokenMutation.isLoading,
    success: getTokenMutation.isSuccess
  };
};
