import { atom } from "recoil";

type AxiosConfig = {
  interceptor?: {
    id: number;
    accessToken: string;
  };
  baseUrlSet: boolean;
};

export const axiosConfigAtom = atom<AxiosConfig>({
  key: "Navtrack:Axios",
  default: { baseUrlSet: false }
});
