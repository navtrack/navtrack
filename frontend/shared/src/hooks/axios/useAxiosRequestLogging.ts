import { AxiosInstance } from "axios";

export const useAxiosRequestLogging = (instance: AxiosInstance) => {
  instance.interceptors.request.use((value) => {
    console.log(value);
    return value;
  });
};
