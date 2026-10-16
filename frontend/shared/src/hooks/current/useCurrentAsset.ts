import { useContext, useMemo } from "react";
import { useAtom } from "jotai";
import { assetConfigurationAtom } from "../../state/assets";
import { useAssetsQuery } from "../queries/assets/useAssetsQuery";
import { CurrentContext } from "./CurrentContext";
import { parseISO } from "date-fns";

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
  const positionDate = currentAsset?.lastPositionMessage?.position?.date;
  const latestDate = useMemo(
    () => (positionDate ? parseISO(positionDate) : new Date()),
    [currentAsset?.id, positionDate]
  );

  return {
    id: current.assetId,
    isLoading: assets.isLoading,
    data: currentAsset,
    latestDate,
    configuration: assetConfiguration,
    setConfiguration: setAssetConfiguration
  };
}
