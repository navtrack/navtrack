import { useQueries } from "@tanstack/react-query";
import { assetsStatsGet, getAssetsStatsGetQueryKey } from "../../../api";
import { AssetStatsPeriod } from "../../../api/model";

export type AssetStatsQueriesProps = {
  assetIds: string[];
  period: AssetStatsPeriod;
};

export function useAssetStatsQueries(props: AssetStatsQueriesProps) {
  const queries = useQueries({
    queries: props.assetIds.map((assetId) => ({
      queryKey: getAssetsStatsGetQueryKey(assetId, props.period),
      queryFn: (q) => assetsStatsGet(assetId, props.period, q.signal),
      staleTime: Infinity
    }))
  });

  return queries;
}
