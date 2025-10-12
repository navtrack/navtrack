import { useEffect } from "react";
import { useAtom, useAtomValue } from "jotai";
import { AUTH_AXIOS_INSTANCE } from "../../../axios/authAxiosInstance";
import { AXIOS_INSTANCE } from "../../../axios/axiosInstance";
import { appConfigAtom } from "../../../state/appConfig";
import { axiosConfigAtom } from "../../../state/axiosConfig";

export function useAxiosBaseUrls() {
  const appConfig = useAtomValue(appConfigAtom);
  const [axiosConfig, setAxiosConfig] = useAtom(axiosConfigAtom);

  useEffect(() => {
    if (appConfig !== undefined) {
      const axiosInstanceInterceptorId =
        AXIOS_INSTANCE.interceptors.request.use((current) => ({
          ...current,
          baseURL: appConfig.api.url
        }));

      const authAxiosInstanceInterceptorId =
        AUTH_AXIOS_INSTANCE.interceptors.request.use((current) => ({
          ...current,
          baseURL: appConfig.api.url
        }));

      setAxiosConfig((x) => ({ ...x, baseUrlSet: true }));

      return () => {
        setAxiosConfig((x) => ({ ...x, baseUrlSet: false }));
        AXIOS_INSTANCE.interceptors.request.eject(axiosInstanceInterceptorId);
        AUTH_AXIOS_INSTANCE.interceptors.request.eject(
          authAxiosInstanceInterceptorId
        );
      };
    }
  }, [appConfig, setAxiosConfig]);

  return axiosConfig.baseUrlSet;
}
