import { atom, selector } from "recoil";
import { AssetModel } from "../api/model/generated";

export const assetsAtom = atom<AssetModel[]>({
  key: "Assets",
  default: []
});

export const scrollToAssetAtom = atom<string | undefined>({
  key: "Assets:ScrollToAtom",
  default: undefined
});

export const currentAssetIdAtom = atom<string | undefined>({
  key: "Assets:CurrentAsset:Id",
  default: undefined
});

export const currentAssetSelector = selector({
  key: "Assets:CurrentAsset",
  get: ({ get }) => {
    const currentAssetId = get(currentAssetIdAtom);
    const assets = get(assetsAtom);

    if (assets) {
      return assets.find((x) => x.shortId === currentAssetId);
    }

    return undefined;
  }
});
