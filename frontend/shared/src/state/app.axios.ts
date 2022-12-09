import { atom } from "recoil";

type AxiosState = {
  interceptorId?: number;
  accessToken?: string;
  initialized: boolean;
};

export const axiosAtom = atom<AxiosState>({
  key: "App:Axios",
  default: { initialized: false }
});
