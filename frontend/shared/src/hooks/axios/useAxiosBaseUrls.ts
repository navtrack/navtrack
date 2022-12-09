import { useEffect, useState } from "react";
import { useRecoilValue } from "recoil";
import { AUTH_AXIOS_INSTANCE } from "../../api/authAxiosInstance";
import { AXIOS_INSTANCE } from "../../api/axiosInstance";
import { configSelector } from "../../state/app.config";

export const useAxiosBaseUrls = () => {
  const config = useRecoilValue(configSelector);
  const [initialized, setInitialized] = useState(false);

  useEffect(() => {
    if (config !== undefined) {
      const axiosInstanceInterceptorId =
        AXIOS_INSTANCE.interceptors.request.use((current) => ({
          ...current,
          baseURL: config.apiUrl
        }));

      const authAxiosInstanceInterceptorId =
        AUTH_AXIOS_INSTANCE.interceptors.request.use((current) => ({
          ...current,
          baseURL: config.apiUrl
        }));

      setInitialized(true);

      return () => {
        setInitialized(false);
        AXIOS_INSTANCE.interceptors.request.eject(axiosInstanceInterceptorId);
        AUTH_AXIOS_INSTANCE.interceptors.request.eject(
          authAxiosInstanceInterceptorId
        );
      };
    }
  }, [config]);

  return initialized;
};
