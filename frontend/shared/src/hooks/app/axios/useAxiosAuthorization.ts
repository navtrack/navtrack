import { useEffect, useState } from "react";
import { useRecoilValue } from "recoil";
import { AUTH_AXIOS_INSTANCE } from "../../../api/authAxiosInstance";
import { isAuthenticatedAtom } from "../../../state/authentication";
import { useAccessToken } from "../authentication/useAccessToken";
import { log } from "../../../utils/log";
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
  const [configured, setConfigured] = useState(false);

  useEffect(() => {
    if (isAuthenticated) {
      if (interceptor !== undefined) {
        AUTH_AXIOS_INSTANCE.interceptors.request.eject(interceptor.requestId);
        AUTH_AXIOS_INSTANCE.interceptors.response.eject(interceptor.responseId);

        setConfigured(false);
        log("AXIOS AUTH", "INTERCEPTORS REMOVED");
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

      interceptor = {
        requestId: requestInterceptorId,
        responseId: responseInterceptorId
      };

      setConfigured(true);
      log("AXIOS AUTH", "INTERCEPTORS SET");
    }
  }, [authentication, isAuthenticated, token]);

  return !isAuthenticated || configured;
}
