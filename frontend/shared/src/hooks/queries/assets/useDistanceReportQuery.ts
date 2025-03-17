import { useAssetsReportsGetDistanceReport } from "../../../api";

export type DistanceReportQueryProps = {
  assetId?: string;
  startDate?: string;
  endDate?: string;
};

export function useDistanceReportQuery(props: DistanceReportQueryProps) {
  const query = useAssetsReportsGetDistanceReport(
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
