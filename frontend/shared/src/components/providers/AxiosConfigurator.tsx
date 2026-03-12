import { Fragment, ReactNode } from "react";
import { appConfigStore } from "../../state/appConfig";
import { AXIOS_INSTANCE } from "../../axios/axiosInstance";
import { AUTH_AXIOS_INSTANCE } from "../../axios/authAxiosInstance";
import { useAuthentication } from "../../hooks/app/authentication/useAuthentication";

type AxiosConfiguratorProps = {
  children?: ReactNode;
};

type AxiosInterceptor = {
  requestId: number;
};

let interceptor: AxiosInterceptor | undefined = undefined;

export function AxiosConfigurator(props: AxiosConfiguratorProps) {
  const authentication = useAuthentication();

  if (
    appConfigStore.config !== undefined &&
    (AXIOS_INSTANCE.defaults.baseURL !== appConfigStore.config.api.url ||
      AUTH_AXIOS_INSTANCE.defaults.baseURL !== appConfigStore.config.api.url)
  ) {
    AXIOS_INSTANCE.defaults.baseURL = appConfigStore.config.api.url;
    AUTH_AXIOS_INSTANCE.defaults.baseURL = appConfigStore.config.api.url;
  }

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
  }

  return <Fragment>{props.children}</Fragment>;
}
