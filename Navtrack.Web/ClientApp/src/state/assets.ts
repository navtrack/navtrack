import { atom, selector } from "recoil";
import { Asset } from "../api/model";

export const assetsAtom = atom<Asset[]>({
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
