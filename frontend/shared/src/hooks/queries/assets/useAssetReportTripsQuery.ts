import {
  getAssetsReportsGetTripReportQueryKey,
  useAssetsReportsGetTripReport
} from "../../../api";

export type TripReportQueryProps = {
  assetId?: string;
  startDate?: string;
  endDate?: string;
};

export function useAssetReportTripsQuery(props: TripReportQueryProps) {
  const query = useAssetsReportsGetTripReport(
    props.assetId as string,
    {
      StartDate: props.startDate,
      EndDate: props.endDate
    },
    {
      query: {
        queryKey: getAssetsReportsGetTripReportQueryKey(`${props.assetId}`, {
          StartDate: props.startDate,
          EndDate: props.endDate
        }),
        enabled: !!props.assetId && !!props.endDate && !!props.startDate,
        refetchOnWindowFocus: false
      }
    }
  );

  return query;
}
