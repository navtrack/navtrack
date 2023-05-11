import { useMemo } from "react";
import { useRecoilValue } from "recoil";
import { assetConfigurationAtom, currentAssetIdAtom } from "../../state/assets";
import { useGetAssetsSignalRQuery } from "../queries/useGetAssetsSignalRQuery";
import { useOnChange } from "../util/useOnChange";

type UseCurrentAssetProps = {
  onChange?: (oldId?: string, newId?: string) => void;
};

export const useCurrentAsset = (props?: UseCurrentAssetProps) => {
  const currentAssetId = useRecoilValue(currentAssetIdAtom);
  const assets = useGetAssetsSignalRQuery();
  const assetConfiguration = useRecoilValue(
    assetConfigurationAtom(currentAssetId)
  );

  const currentAsset = useMemo(
    () => assets.data?.items.find((x) => x.id === currentAssetId),

    [assets, currentAssetId]
  );

  useOnChange(currentAssetId, (prev, cur) => props?.onChange?.(prev, cur));

  return {
    isLoading: assets.isLoading,
    data: currentAsset,
    configuration: assetConfiguration
  };
};
