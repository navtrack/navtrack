import {
  getAssetsReportsGetDistanceReportQueryKey,
  useAssetsReportsGetDistanceReport
} from "../../../api";

export type OrganizationReportDistanceQuery = {
  organizationId?: string;
  startDate?: string;
  endDate?: string;
};

export function useOrganizationReportDistanceQuery(
  props: OrganizationReportDistanceQuery
) {
  const query = useAssetsReportsGetDistanceReport(
    props.organizationId as string,
    {
      StartDate: props.startDate,
      EndDate: props.endDate
    },
    {
      query: {
        queryKey: getAssetsReportsGetDistanceReportQueryKey(
          `${props.organizationId}`,
          {
            StartDate: props.startDate,
            EndDate: props.endDate
          }
        ),
        enabled: !!props.organizationId && !!props.endDate && !!props.startDate,
        refetchOnWindowFocus: false
      }
    }
  );

  return query;
}
