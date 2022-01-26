import { atom } from "recoil";
import { CurrentUserModel } from "../api/model/generated";

export const currentUserAtom = atom<CurrentUserModel | undefined>({
  key: "CurrentUser",
  default: undefined
});
