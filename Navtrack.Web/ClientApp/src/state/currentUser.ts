import { atom } from "recoil";
import { CurrentUser } from "../api/model";

export const currentUserAtom = atom<CurrentUser | undefined>({
  key: "CurrentUser",
  default: undefined
});
