import { atom, atomFamily } from "recoil";
import { getLocalStorageEffect } from "./getLocalStorageEffect";

export const scrollToAssetAtom = atom<string | undefined>({
  key: "Navtrack:Assets:ScrollToAtom",
  default: undefined
});

export const currentAssetIdAtom = atom<string | undefined>({
  key: "Navtrack:Assets:CurrentAsset:Id",
  default: undefined,
  effects: [getLocalStorageEffect<string | undefined>()]
});

type AssetConfiguration = {
  liveTracking: LiveTracking;
};

type LiveTracking = {
  follow: boolean;
  zoom: number;
};

export const assetConfigurationAtom = atomFamily<
  AssetConfiguration,
  string | undefined
>({
  key: "Navtrack:Assets:Configuration",
  default: {
    liveTracking: {
      follow: true,
      zoom: 16
    }
  }
});
