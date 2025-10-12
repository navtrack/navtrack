import { atom } from "jotai";

type AxiosConfig = {
  interceptor?: {
    id: number;
    accessToken: string;
  };
  baseUrlSet: boolean;
};

export const axiosConfigAtom = atom<AxiosConfig>({ baseUrlSet: false });
