import { Atom, atom } from "jotai";
import { atomFamily } from "jotai/utils";

export const scrollToAssetAtom = atom<string | undefined>(undefined);

type AssetConfiguration = {
  liveTracking: LiveTracking;
};

type LiveTracking = {
  follow: boolean;
  zoom: number;
};

export const assetConfigurationAtom = atomFamily(() =>
  atom<AssetConfiguration>({
    liveTracking: {
      follow: true,
      zoom: 16
    }
  })
);
