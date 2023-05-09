import { useEffect } from "react";
import { useRecoilState, useRecoilValue } from "recoil";
import { AUTH_AXIOS_INSTANCE } from "../../../api/authAxiosInstance";
import { authenticationAtom } from "../../../state/authentication";
import { axiosConfigAtom } from "../../../state/axiosConfig";

export const useAxiosAuthorization = () => {
  const authentication = useRecoilValue(authenticationAtom);
  const [axiosAuthState, setAxiosAuthState] = useRecoilState(axiosConfigAtom);

  useEffect(() => {
    if (
      authentication.isAuthenticated &&
      authentication.token?.accessToken !== undefined &&
      authentication.token?.accessToken !== axiosAuthState?.accessToken
    ) {
      if (axiosAuthState.accessTokenInterceptorId !== undefined) {
        AUTH_AXIOS_INSTANCE.interceptors.request.eject(
          axiosAuthState.accessTokenInterceptorId
        );
      }

      const newInterceptorId = AUTH_AXIOS_INSTANCE.interceptors.request.use(
        (config) => {
          config.headers.Authorization = `Bearer ${authentication.token?.accessToken}`;

          return config;
        }
      );

      setAxiosAuthState((x) => ({
        ...x,
        accessTokenInterceptorId: newInterceptorId,
        accessToken: authentication.token?.accessToken,
        accessTokenSet: true
      }));
    }
  }, [
    authentication.isAuthenticated,
    authentication.token?.accessToken,
    axiosAuthState,
    setAxiosAuthState
  ]);
};
