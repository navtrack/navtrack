import { atom } from "recoil";

type AxiosConfig = {
  accessTokenInterceptorId?: number;
  accessToken?: string;
  accessTokenSet: boolean;
  baseUrlSet: boolean;
};

export const axiosConfigAtom = atom<AxiosConfig>({
  key: "Navtrack:Axios",
  default: { accessTokenSet: false, baseUrlSet: false }
});
