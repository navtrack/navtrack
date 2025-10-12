import {
  getAssetsReportsGetFuelConsumptionReportQueryKey,
  useAssetsReportsGetFuelConsumptionReport
} from "../../../api";

export type DistanceReportQueryProps = {
  assetId?: string;
  startDate?: string;
  endDate?: string;
};

export function useAssetReportFuelConsumptionQuery(
  props: DistanceReportQueryProps
) {
  const query = useAssetsReportsGetFuelConsumptionReport(
    props.assetId as string,
    {
      StartDate: props.startDate,
      EndDate: props.endDate
    },
    {
      query: {
        queryKey: getAssetsReportsGetFuelConsumptionReportQueryKey(
          props.assetId as string,
          {
            StartDate: props.startDate,
            EndDate: props.endDate
          }
        ),
        enabled: !!props.assetId && !!props.endDate && !!props.startDate,
        refetchOnWindowFocus: false
      }
    }
  );

  return query;
}
