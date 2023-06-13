import { useGetAssetsAssetIdReportsTimeDistance } from "../../api/index-generated";

export type UseTimeAndDistanceReportQueryProps = {
  assetId?: string;
  startDate?: string;
  endDate?: string;
};

export const useTimeAndDistanceReportQuery = (
  props: UseTimeAndDistanceReportQueryProps
) => {
  const query = useGetAssetsAssetIdReportsTimeDistance(
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
};
