import { atom } from "recoil";
import { AssetModel } from "../api/model/generated";

export const currentAssetAtom = atom<AssetModel | undefined>({
  key: "CurrentAsset",
  default: undefined
});
