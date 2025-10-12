import {
  getAssetsReportsGetDistanceReportQueryKey,
  useAssetsReportsGetDistanceReport
} from "../../../api";

export type AssetReportReportQueryProps = {
  assetId?: string;
  startDate?: string;
  endDate?: string;
};

export function useAssetReportDistanceQuery(
  props: AssetReportReportQueryProps
) {
  const query = useAssetsReportsGetDistanceReport(
    props.assetId as string,
    {
      StartDate: props.startDate,
      EndDate: props.endDate
    },
    {
      query: {
        queryKey: getAssetsReportsGetDistanceReportQueryKey(
          `${props.assetId}`,
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
