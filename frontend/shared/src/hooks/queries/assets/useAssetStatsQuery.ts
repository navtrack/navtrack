import { getAssetsStatsGetQueryKey, useAssetsStatsGet } from "../../../api";
import { AssetStatsPeriod } from "../../../api/model";

export type UseAssetStatsQueryProps = {
  assetId?: string;
  period: AssetStatsPeriod;
};

export const useAssetStatsQuery = (props: UseAssetStatsQueryProps) => {
  console.log(props.period);

  const query = useAssetsStatsGet(props.assetId as string, props.period, {
    query: {
      queryKey: getAssetsStatsGetQueryKey(props.assetId, props.period),
      enabled: !!props.assetId,
      refetchOnWindowFocus: false
    }
  });

  return query;
};
