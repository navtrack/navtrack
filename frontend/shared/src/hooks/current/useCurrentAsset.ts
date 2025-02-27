import { useEffect, useMemo } from "react";
import { useRecoilState } from "recoil";
import { assetConfigurationAtom } from "../../state/assets";
import { useAssetsQuery } from "../queries/assets/useAssetsQuery";
import {
  currentAssetIdAtom,
  currentAssetIdInitializedAtom
} from "../../state/current";
import { useAssetQuery } from "../queries/assets/useAssetQuery";
import { useCurrentOrganization } from "./useCurrentOrganization";

export function useCurrentAsset() {
  const [currentAssetId, setCurrentAssetId] =
    useRecoilState(currentAssetIdAtom);
  const [currentAssetIdInitialized, setCurrentAssetIdInitialized] =
    useRecoilState(currentAssetIdInitializedAtom);

  const currentOrganizationId = useCurrentOrganization();
  const asset = useAssetQuery({
    assetId: !!currentAssetId ? currentAssetId : undefined
  });
  const assets = useAssetsQuery({
    organizationId: currentOrganizationId.id ?? asset.data?.organizationId
  });
  const [assetConfiguration, setAssetConfiguration] = useRecoilState(
    assetConfigurationAtom(currentAssetId)
  );

  const currentAsset = useMemo(
    () => assets.data?.items.find((x) => x.id === currentAssetId),

    [assets, currentAssetId]
  );

  useEffect(() => {
    if (currentAssetId !== null && !currentAssetIdInitialized) {
      setCurrentAssetIdInitialized(true);
    }
  }, [
    currentAssetId,
    currentAssetIdInitialized,
    currentOrganizationId,
    setCurrentAssetIdInitialized
  ]);

  return {
    id: currentAssetId,
    isLoading: assets.isLoading,
    initialized: currentAssetIdInitialized,
    data: currentAsset,
    configuration: assetConfiguration,
    setConfiguration: setAssetConfiguration,
    setId: (value: string | undefined) => setCurrentAssetId(value),
    reset: () => {
      setCurrentAssetId(undefined);
    }
  };
}
