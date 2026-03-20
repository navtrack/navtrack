import { TableV2 } from "../../ui/table/TableV2";
import { useShow } from "@navtrack/shared/hooks/util/useShow";
import { useAtomValue } from "jotai";
import { Card } from "../../ui/card/Card";
import { CustomBarChart } from "../../ui/charts/bar/CustomBarChart";
import { useElementSize } from "@navtrack/shared/hooks/util/useElementSize";
import { useRef } from "react";
import { ButtonGroup } from "../../ui/button/ButtonGroup";
import { useLocationFilterKey } from "../../asset/shared/location-filter/useLocationFilterKey";
import { locationFiltersSelector } from "../../asset/shared/location-filter/locationFilterState";
import { LocationFilter } from "../../asset/shared/location-filter/LocationFilter";
import { useAssetsQuery } from "@navtrack/shared/hooks/queries/assets/useAssetsQuery";
import { useCurrentOrganization } from "@navtrack/shared/hooks/current/useCurrentOrganization";
import { DistanceReportItemModel } from "@navtrack/shared/api/model";
import { useDistanceReport } from "@navtrack/shared/hooks/reports/useDistanceReport";
import {
  DistanceReportChartItem,
  useDistanceReportChart
} from "@navtrack/shared/hooks/reports/useDistanceReportChart";

export function OrganizationReportsDistancePage() {
  const show = useShow();
  const currentOrganization = useCurrentOrganization();
  const locationFilterKey = useLocationFilterKey("reports-distance");
  const filters = useAtomValue(locationFiltersSelector(locationFilterKey));

  const tableRef = useRef<HTMLDivElement>(null);
  const tableSize = useElementSize(tableRef);

  const assets = useAssetsQuery({
    organizationId: currentOrganization.data?.id
  });

  const distanceReport = useDistanceReport({
    assetIds: assets.data?.items.map((asset) => asset.id) || [],
    startDate: filters.startDate,
    endDate: filters.endDate
  });
  const distanceReportChart = useDistanceReportChart({
    items: distanceReport.result.items
  });

  return (
    <>
      <LocationFilter filterPage="reports-distance" />
      <div className="h-full flex flex-col">
        <div className="flex-1" ref={tableRef}>
          <TableV2<DistanceReportItemModel>
            height={tableSize.height}
            rows={distanceReport.result.items}
            columns={[
              {
                labelId: "generic.date",
                row: (item) => <>{show.date(item.date)}</>,
                value: (item) => item.date,
                sort: "desc"
              },
              {
                labelId: "generic.distance",
                row: (item) => <>{show.distance(item.distance) ?? "-"}</>,
                footer: () => (
                  <span className="font-semibold">
                    {show.distance(distanceReport.result.totalDistance)}
                  </span>
                ),
                value: (item) => item.distance
              },
              {
                labelId: "generic.duration",
                row: (item) => <>{show.duration(item.duration) ?? "-"}</>,
                footer: () => (
                  <span className="font-semibold">
                    {show.duration(distanceReport.result.totalDuration)}
                  </span>
                ),
                value: (item) => item.duration
              },
              {
                labelId: "generic.average-speed",
                row: (item) => <>{show.speed(item.averageSpeed) ?? "-"}</>,
                footer: () => (
                  <span className="font-semibold">
                    {show.speed(distanceReport.result.averageSpeed)}
                  </span>
                ),
                value: (item) => item.averageSpeed
              },
              {
                labelId: "generic.max-speed",
                row: (item) => <>{show.speed(item.maxSpeed) ?? "-"}</>,
                footer: () => (
                  <span className="font-semibold">
                    {show.speed(distanceReport.result.maxSpeed)}
                  </span>
                ),
                value: (item) => item.maxSpeed
              }
            ]}
          />
        </div>
        <Card className="flex-1 p-2 mt-4 flex flex-col">
          <div className="flex justify-center">
            <ButtonGroup
              options={distanceReportChart.distanceChartOptions}
              onChange={distanceReportChart.onChartTypeChange}
            />
          </div>
          <div className="mt-4 flex-1">
            <CustomBarChart<DistanceReportChartItem>
              {...distanceReportChart.chartConfig}
              data={distanceReportChart.chartItems}
              hideLegend
            />
          </div>
        </Card>
      </div>
    </>
  );
}
