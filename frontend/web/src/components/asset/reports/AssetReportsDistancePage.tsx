import { LocationFilter } from "../shared/location-filter/LocationFilter";
import { useCurrentAsset } from "@navtrack/shared/hooks/current/useCurrentAsset";
import { TableV2 } from "../../ui/table/TableV2";
import { DistanceReportItemModel } from "@navtrack/shared/api/model";
import { useShow } from "@navtrack/shared/hooks/util/useShow";
import { FormattedMessage } from "react-intl";
import { useAtomValue } from "jotai";
import { locationFiltersSelector } from "../shared/location-filter/locationFilterState";
import { useLocationFilterKey } from "../shared/location-filter/useLocationFilterKey";
import { useAssetReportDistanceQuery } from "@navtrack/shared/hooks/queries/assets/useAssetReportDistanceQuery";
import { AreaChart } from "../../ui/charts/AreaChart";
import { Card } from "../../ui/card/Card";

export function AssetReportsDistancePage() {
  const show = useShow();
  const currentAsset = useCurrentAsset();
  const locationFilterKey = useLocationFilterKey("reports-distance");
  const filters = useAtomValue(locationFiltersSelector(locationFilterKey));
  const distanceReport = useAssetReportDistanceQuery({
    assetId: currentAsset.data?.id,
    startDate: filters.startDate,
    endDate: filters.endDate
  });

  const data =
    distanceReport.data?.items.map((item) => ({
      date: show.date(item.date),
      distance: item.distance
    })) ?? [];

  return (
    <>
      <LocationFilter filterPage="reports-distance" />
      <Card className="p-4">
        <AreaChart
          className="h-60"
          data={data}
          index="date"
          categories={["distance"]}
          valueFormatter={(value) => show.distance(value) ?? "-"}
          onValueChange={(v) => console.log(v)}
        />
      </Card>
      <TableV2<DistanceReportItemModel>
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
