import { atom, selector } from "recoil";

export type ConfigState = {
  config?: Config;
  initialized: boolean;
};

export type Config = {
  apiUrl: string;
};

export const configAtom = atom<ConfigState>({
  key: "App:Config",
  default: { initialized: false }
});

export const configSelector = selector({
  key: "App:Config:Selector",
  get: ({ get }) => {
    const state = get(configAtom);

    return state.config;
  }
});
