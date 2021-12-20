import { atom } from "recoil";
import { Asset } from "../api/model";

export const currentAssetAtom = atom<Asset | undefined>({
  key: "CurrentAsset",
  default: undefined
});
