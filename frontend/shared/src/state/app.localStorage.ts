import { atom } from "recoil";
import { Token } from "./authentication";

export type LocalStorageState = {
  initialized: boolean;
  data?: LocalStorageData;
};

export type LocalStorageData = {
  token?: Token;
};

export const localStorageAtom = atom<LocalStorageState>({
  key: "App:LocalStorage",
  default: { initialized: false }
});
