import { atom } from "recoil";

export type AppConfig = {
  apiUrl: string;
};

export const appConfigAtom = atom<AppConfig | undefined>({
  key: "Navtrack:AppConfig",
  default: undefined
});
