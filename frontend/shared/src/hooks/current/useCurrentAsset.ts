import { useMemo } from "react";
import { useRecoilValue } from "recoil";
import { assetConfigurationAtom } from "../../state/assets";
import { useAssetsQuery } from "../queries/assets/useAssetsQuery";
import { useOnChange } from "../util/useOnChange";
import {
  currentAssetIdAtom,
  currentOrganizationIdAtom
} from "../../state/current";
import { useAssetQuery } from "../queries/assets/useAssetQuery";

type UseCurrentAssetProps = {
  onChange?: (oldId?: string, newId?: string) => void;
};

export function useCurrentAsset(props?: UseCurrentAssetProps) {
  const currentAssetId = useRecoilValue(currentAssetIdAtom);
  const currentOrganizationId = useRecoilValue(currentOrganizationIdAtom);
  const asset = useAssetQuery({
    assetId: currentOrganizationId === undefined ? currentAssetId : undefined
  });
  const assets = useAssetsQuery({
    organizationId: currentOrganizationId ?? asset.data?.organizationId
  });
  const assetConfiguration = useRecoilValue(
    assetConfigurationAtom(currentAssetId)
  );

  const currentAsset = useMemo(
    () => assets.data?.items.find((x) => x.id === currentAssetId),

    [assets, currentAssetId]
  );

  useOnChange(currentAssetId, (prev, cur) => props?.onChange?.(prev, cur));

  return {
    id: currentAssetId,
    isLoading: assets.isLoading,
    data: currentAsset,
    configuration: assetConfiguration
  };
}
