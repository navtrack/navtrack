import { useEffect } from "react";
import { useRecoilState } from "recoil";
import { AUTH_AXIOS_INSTANCE } from "./authAxiosInstance";
import { axiosAuthAtom } from "./axiosAuthAtom";
import useAppContext from "../hooks/app/useAppContext";

export default function useAxiosAuthorization() {
  const { appContext } = useAppContext();
  const [axiosAuthState, setAxiosAuthState] = useRecoilState(axiosAuthAtom);

  useEffect(() => {
    if (
      appContext.initialized &&
      appContext.isAuthenticated &&
      appContext.token?.accessToken !== undefined &&
      appContext.token?.accessToken !== axiosAuthState?.accessToken
    ) {
      if (axiosAuthState !== undefined) {
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
              Authorization: `Bearer ${appContext.token?.accessToken}`
            }
          };
        }
      );

      setAxiosAuthState({
        interceptorId: newInterceptorId,
        accessToken: appContext.token?.accessToken,
        interceptorInit: true
      });
    }
  }, [
    appContext.initialized,
    appContext.isAuthenticated,
    appContext.token?.accessToken,
    axiosAuthState,
    axiosAuthState?.accessToken,
    setAxiosAuthState
  ]);
}
