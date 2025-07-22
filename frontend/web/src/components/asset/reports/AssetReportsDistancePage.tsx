import { LocationFilter } from "../shared/location-filter/LocationFilter";
import { useCurrentAsset } from "@navtrack/shared/hooks/current/useCurrentAsset";
import { TableV2 } from "../../ui/table/TableV2";
import { DistanceReportItem } from "@navtrack/shared/api/model";
import { useShow } from "@navtrack/shared/hooks/util/useShow";
import { FormattedMessage } from "react-intl";
import { useRecoilValue } from "recoil";
import { locationFiltersSelector } from "../shared/location-filter/locationFilterState";
import { useLocationFilterKey } from "../shared/location-filter/useLocationFilterKey";
import { useAssetReportDistanceQuery } from "@navtrack/shared/hooks/queries/assets/useAssetReportDistanceQuery";

export function AssetReportsDistancePage() {
  const show = useShow();
  const currentAsset = useCurrentAsset();
  const locationFilterKey = useLocationFilterKey("reports-distance");
  const filters = useRecoilValue(locationFiltersSelector(locationFilterKey));
  const distanceReport = useAssetReportDistanceQuery({
    assetId: currentAsset.data?.id,
    startDate: filters.startDate,
    endDate: filters.endDate
  });

  return (
    <>
      <LocationFilter filterPage="reports-distance" />
      <TableV2<DistanceReportItem>
        className="h-full"
        columns={[
          {
            labelId: "generic.date",
            row: (item) => <>{show.date(item.date)}</>,
            footer: () => <FormattedMessage id="generic.total" />,
            sortValue: (item) => item.date,
            sortable: true,
            sort: "desc"
          },
          {
            labelId: "generic.distance",
            row: (item) => <>{show.distance(item.distance) ?? "-"}</>,
            footer: () => (
              <span className="font-semibold">
                {show.distance(distanceReport.data?.totalDistance)}
              </span>
            ),
            sortValue: (item) => item.distance,
            sortable: true
          },
          {
            labelId: "generic.duration",
            row: (item) => <>{show.duration(item.duration) ?? "-"}</>,
            footer: () => (
              <span className="font-semibold">
                {show.duration(distanceReport.data?.totalDuration)}
              </span>
            ),
            sortValue: (item) => item.duration,
            sortable: true
          },
          {
            labelId: "generic.average-speed",
            row: (item) => <>{show.speed(item.averageSpeed) ?? "-"}</>,
            footer: () => (
              <span className="font-semibold">
                {show.speed(distanceReport.data?.averageSpeed)}
              </span>
            ),
            sortValue: (item) => item.averageSpeed,
            sortable: true
          },
          {
            labelId: "generic.max-speed",
            row: (item) => <>{show.speed(item.maxSpeed) ?? "-"}</>,
            footer: () => (
              <span className="font-semibold">
                {show.speed(distanceReport.data?.maxSpeed)}
              </span>
            ),
            sortValue: (item) => item.maxSpeed,
            sortable: true
          }
        ]}
        rows={distanceReport.data?.items}
      />
    </>
  );
}
