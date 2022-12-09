import { useEffect } from "react";
import { useRecoilState, useRecoilValue } from "recoil";
import { AUTH_AXIOS_INSTANCE } from "../../api/authAxiosInstance";
import { authenticationAtom } from "../../state/authentication";
import { axiosAtom } from "../../state/app.axios";

export const useAxiosAuthorization = () => {
  const authentication = useRecoilValue(authenticationAtom);
  const [axiosAuthState, setAxiosAuthState] = useRecoilState(axiosAtom);

  useEffect(() => {
    if (
      authentication.isAuthenticated &&
      authentication.token?.accessToken !== undefined &&
      authentication.token?.accessToken !== axiosAuthState?.accessToken
    ) {
      if (axiosAuthState.interceptorId !== undefined) {
        AUTH_AXIOS_INSTANCE.interceptors.request.eject(
          axiosAuthState.interceptorId
        );
      }

      const newInterceptorId = AUTH_AXIOS_INSTANCE.interceptors.request.use(
        (config) => {
          return {
            ...config,
            headers: {
              ...config.headers,
              Authorization: `Bearer ${authentication.token?.accessToken}`
            }
          };
        }
      );

      setAxiosAuthState({
        interceptorId: newInterceptorId,
        accessToken: authentication.token?.accessToken,
        initialized: true
      });
    }
  }, [
    authentication.isAuthenticated,
    authentication.token?.accessToken,
    axiosAuthState,
    setAxiosAuthState
  ]);
};
