import { AxiosInstance } from "axios";

export const useAxiosResponseLogging = (instance: AxiosInstance) => {
  instance.interceptors.response.use((value) => {
    console.log(value);
    return value;
  });
};
