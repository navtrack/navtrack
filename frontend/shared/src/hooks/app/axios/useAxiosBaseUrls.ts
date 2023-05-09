import { useEffect } from "react";
import { useRecoilState, useRecoilValue } from "recoil";
import { AUTH_AXIOS_INSTANCE } from "../../../api/authAxiosInstance";
import { AXIOS_INSTANCE } from "../../../api/axiosInstance";
import { appConfigAtom } from "../../../state/appConfig";
import { axiosConfigAtom } from "../../../state/axiosConfig";

export function useAxiosBaseUrls() {
  const appConfig = useRecoilValue(appConfigAtom);
  const [axiosConfig, setAxiosConfig] = useRecoilState(axiosConfigAtom);

  useEffect(() => {
    if (appConfig !== undefined) {
      const axiosInstanceInterceptorId =
        AXIOS_INSTANCE.interceptors.request.use((current) => ({
          ...current,
          baseURL: appConfig.apiUrl
        }));

      const authAxiosInstanceInterceptorId =
        AUTH_AXIOS_INSTANCE.interceptors.request.use((current) => ({
          ...current,
          baseURL: appConfig.apiUrl
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
