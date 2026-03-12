import { useQueries } from "@tanstack/react-query";
import {
  assetsReportsGetFuelConsumptionReport,
  getAssetsReportsGetFuelConsumptionReportQueryKey
} from "../../../api";

export type AssetFuelConsumptionReportQueriesProps = {
  assetIds: string[];
  startDate?: string;
  endDate?: string;
};

export function useAssetFuelConsumptionReportsQueries(
  props: AssetFuelConsumptionReportQueriesProps
) {
  const queries = useQueries({
    queries: props.assetIds.map((assetId) => ({
      queryKey: getAssetsReportsGetFuelConsumptionReportQueryKey(assetId, {
        StartDate: props.startDate,
        EndDate: props.endDate
      }),
      queryFn: (q) =>
        assetsReportsGetFuelConsumptionReport(
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
