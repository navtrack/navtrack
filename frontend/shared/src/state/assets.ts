import { atom, atomFamily } from "recoil";

export const scrollToAssetAtom = atom<string | undefined>({
  key: "Navtrack:Assets:ScrollToAtom",
  default: undefined
});

export const currentAssetIdAtom = atom<string | undefined>({
  key: "Navtrack:Assets:CurrentAsset:Id",
  default: undefined
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
