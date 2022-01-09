import { useEffect } from "react";
import { useRecoilState, useRecoilValue } from "recoil";
import { AUTH_AXIOS_INSTANCE } from "./authAxiosInstance";
import { axiosAtom } from "../state/app.axios";
import { authenticationAtom } from "../state/app.authentication";

export default function useAxiosAuthorization() {
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
}
