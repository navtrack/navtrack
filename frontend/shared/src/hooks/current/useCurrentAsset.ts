import { useContext, useMemo } from "react";
import { useAtom } from "jotai";
import { assetConfigurationAtom } from "../../state/assets";
import { useAssetsQuery } from "../queries/assets/useAssetsQuery";
import { CurrentContext } from "./CurrentContext";

export function useCurrentAsset() {
  const current = useContext(CurrentContext);

  const assets = useAssetsQuery({ organizationId: current.organizationId });
  const [assetConfiguration, setAssetConfiguration] = useAtom(
    assetConfigurationAtom(current.assetId)
  );

  const currentAsset = useMemo(
    () => (assets.data?.items ?? []).find((x) => x.id === current.assetId),
    [assets.data?.items, current.assetId]
  );

  return {
    id: current.assetId,
    isLoading: assets.isLoading,
    data: currentAsset,
    configuration: assetConfiguration,
    setConfiguration: setAssetConfiguration
  };
}
