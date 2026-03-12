import { useQueries } from "@tanstack/react-query";
import {
  assetsReportsGetDistanceReport,
  getAssetsReportsGetDistanceReportQueryKey
} from "../../../api";

export type AssetReportReportQueriesProps = {
  assetIds: string[];
  startDate?: string;
  endDate?: string;
};

export function useAssetReportDistanceQueries(
  props: AssetReportReportQueriesProps
) {
  const queries = useQueries({
    queries: props.assetIds.map((assetId) => ({
      queryKey: getAssetsReportsGetDistanceReportQueryKey(assetId, {
        StartDate: props.startDate,
        EndDate: props.endDate
      }),
      queryFn: (q) =>
        assetsReportsGetDistanceReport(
          assetId,
          {
            StartDate: props.startDate,
            EndDate: props.endDate
          },
          q.signal
        ),
      staleTime: Infinity
    }))
  });

  return queries;
}
