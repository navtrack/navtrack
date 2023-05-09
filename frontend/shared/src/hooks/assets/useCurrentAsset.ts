import { useMemo } from "react";
import { useRecoilValue } from "recoil";
import { assetConfigurationAtom, currentAssetIdAtom } from "../../state/assets";
import { useGetAssetsSignalRQuery } from "../queries/useGetAssetsSignalRQuery";

export const useCurrentAsset = () => {
  const currentAssetId = useRecoilValue(currentAssetIdAtom);
  const assets = useGetAssetsSignalRQuery();
  const assetConfiguration = useRecoilValue(
    assetConfigurationAtom(currentAssetId)
  );

  const currentAsset = useMemo(
    () => assets.data?.items.find((x) => x.id === currentAssetId),

    [assets, currentAssetId]
  );

  return {
    isLoading: assets.isLoading,
    data: currentAsset,
    configuration: assetConfiguration
  };
};
