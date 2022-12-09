import { useEffect, useMemo } from "react";
import { useRecoilState } from "recoil";
import { currentAssetIdAtom } from "../../state/assets";
import { useGetAssets } from "./useGetAssets";

export const useCurrentAsset = () => {
  const [currentAssetId, setCurrentAssetId] =
    useRecoilState(currentAssetIdAtom);
  const assets = useGetAssets();

  const currentAsset = useMemo(() => {
    if (assets.length > 0 && currentAssetId) {
      return assets.find((x) => x.shortId === currentAssetId);
    }

    return undefined;
  }, [assets, currentAssetId]);

  // useEffect(() => {
  //   if (!currentAssetId && assets.length > 0) {
  //     setCurrentAssetId(assets[0].shortId);
  //   }
  // }, [assets, currentAssetId, setCurrentAssetId]);

  return currentAsset;
};
