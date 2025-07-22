import { useAssetsReportsGetTripReport } from "../../../api";

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
        enabled: !!props.assetId && !!props.endDate && !!props.startDate,
        refetchOnWindowFocus: false
      }
    }
  );

  return query;
}
