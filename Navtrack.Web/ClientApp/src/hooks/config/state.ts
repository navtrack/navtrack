import { atom } from "recoil";
import { ConfigState } from "./types";

export const configState = atom<ConfigState>({
  key: "Settings",
  default: {}
});
