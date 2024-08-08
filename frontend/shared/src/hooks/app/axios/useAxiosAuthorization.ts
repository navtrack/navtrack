import { useEffect, useState } from "react";
import { AUTH_AXIOS_INSTANCE } from "../../../api/authAxiosInstance";
import { log } from "../../../utils/log";
import { useAuthentication } from "../authentication/useAuthentication";

type AxiosInterceptor = {
  requestId: number;
};

let interceptor: AxiosInterceptor | undefined = undefined;

export function useAxiosAuthorization() {
  const authentication = useAuthentication();
  const [configured, setConfigured] = useState(false);

  useEffect(() => {
    if (interceptor === undefined) {
      const requestInterceptorId = AUTH_AXIOS_INSTANCE.interceptors.request.use(
        async (config) => {
          const accessToken = await authentication.getAccessToken();

          config.headers.Authorization = `Bearer ${accessToken}`;

          return config;
        }
      );

      interceptor = {
        requestId: requestInterceptorId
      };

      setConfigured(true);
      log("AXIOS AUTH", "INTERCEPTORS SET");
    }
  }, [authentication]);

  return configured;
}
