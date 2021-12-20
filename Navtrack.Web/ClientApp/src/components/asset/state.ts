import { atomFamily } from "recoil";
import { AssetConfiguration } from "./types";

export const assetConfiguration = atomFamily<AssetConfiguration, string | undefined>({
  key: "AssetConfiguration",
  default: {
    liveTracking: {
      follow: true,
      zoom: 16
    }
  }
});

// export const assetsAtom = atom<Asset[]>({
//   key: "Assets",
//   default: []
// });

// export const currentAssetIdAtom = atom<string | undefined>({
//   key: "Assets:CurrentAsset:Id",
//   default: undefined
// });

// export const currentAssetSelector = selector({
//   key: "Assets:CurrentAsset",
//   get: ({ get }) => {
//     const currentAssetId = get(currentAssetIdAtom);
//     const assets = get(assetsAtom);

//     if (assets) {
//       return assets.find((x) => x.shortId === currentAssetId);
//     }

//     return undefined;
//   }
//   // set: ({ set }, newValue) => {
//   //   // if (newValue instanceof DefaultValue) {
//   //   //   set(orderListAmountMinAtom, newValue);
//   //   //   set(orderListAmountMaxAtom, newValue);
//   //   //   return;
//   //   // }
//   //   // set(orderListAmountMinAtom, newValue.MinTotalAmountInTax);
//   //   // set(orderListAmountMaxAtom, newValue.MaxTotalAmountInTax);
//   // }
// });
