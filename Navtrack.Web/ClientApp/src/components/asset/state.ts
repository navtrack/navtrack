import { atomFamily } from "recoil";
import { AssetConfiguration } from "./types";

export const assetConfiguration = atomFamily<
  AssetConfiguration,
  string | undefined
>({
  key: "AssetConfiguration",
  default: {
    liveTracking: {
      follow: true,
      zoom: 16
    }
  }
});
