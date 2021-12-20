import { atom } from "recoil";

type Token = {
  accessToken: string;
  refreshToken: string;
  expiryDate: Date;
};

export type AppContext = {
  initialized: boolean;
  isAuthenticated?: boolean;
  token?: Token;
};

export const appContextAtom = atom<AppContext>({
  key: "AppContext",
  default: {
    initialized: false,
    isAuthenticated: false
  }
});
