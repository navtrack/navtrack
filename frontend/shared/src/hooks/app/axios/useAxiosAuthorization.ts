import { useEffect } from "react";
import { useRecoilValue } from "recoil";
import { AUTH_AXIOS_INSTANCE } from "../../../api/authAxiosInstance";
import { isAuthenticatedAtom } from "../../../state/authentication";
import { useAccessToken } from "../authentication/useAccessToken";
import { LogLevel, log } from "../../../utils/log";
import { AxiosError } from "axios";
import { useAuthentication } from "../authentication/useAuthentication";
import { AuthenticationErrorType } from "../authentication/authentication";

type AxiosInterceptor = {
  requestId: number;
  responseId: number;
};

let interceptor: AxiosInterceptor | undefined = undefined;

export function useAxiosAuthorization() {
  const isAuthenticated = useRecoilValue(isAuthenticatedAtom);
  const token = useAccessToken();
  const authentication = useAuthentication();

  useEffect(() => {
    if (isAuthenticated) {
      if (interceptor !== undefined) {
        AUTH_AXIOS_INSTANCE.interceptors.request.eject(interceptor.requestId);
        AUTH_AXIOS_INSTANCE.interceptors.response.eject(interceptor.responseId);
        log(LogLevel.DEBUG, "INTERCEPTOR REMOVED", interceptor?.requestId);
      }

      const requestInterceptorId = AUTH_AXIOS_INSTANCE.interceptors.request.use(
        async (config) => {
          const accessToken = await token.getAccessToken();

          if (accessToken === undefined) {
            await authentication.clear();
          }

          config.headers.Authorization = `Bearer ${accessToken}`;

          return config;
        }
      );
      const responseInterceptorId =
        AUTH_AXIOS_INSTANCE.interceptors.response.use(
          async (response) => {
            return response;
          },
          async function (error: AxiosError) {
            if (
              error.response?.status === 401 ||
              error.response?.status === 400
            ) {
              await authentication.clear(AuthenticationErrorType.Other);
            }

            return Promise.reject(error);
          }
        );
      log(LogLevel.DEBUG, "INTERCEPTOR SET");

      interceptor = {
        requestId: requestInterceptorId,
        responseId: responseInterceptorId
      };
    }
  }, [authentication, isAuthenticated, token]);
}
