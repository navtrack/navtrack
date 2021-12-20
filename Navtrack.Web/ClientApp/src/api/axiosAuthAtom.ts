import { atom } from "recoil";

type AxiosAuthentication = {
  interceptorId: number;
  accessToken: string;
  interceptorInit: boolean;
};

export const axiosAuthAtom = atom<AxiosAuthentication | undefined>({
  key: "Axios:Authentication",
  default: undefined
});
