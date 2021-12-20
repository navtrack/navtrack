import { useEffect } from "react";
import { useConfig } from "../hooks/config/useConfig";
import { AUTH_AXIOS_INSTANCE } from "./authAxiosInstance";
import { AXIOS_INSTANCE } from "./axiosInstance";

export const useAxiosBaseUrls = () => {
  const config = useConfig();

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

      return () => {
        AXIOS_INSTANCE.interceptors.request.eject(axiosInstanceInterceptorId);
        AUTH_AXIOS_INSTANCE.interceptors.request.eject(
          authAxiosInstanceInterceptorId
        );
      };
    }
  }, [config]);
};
