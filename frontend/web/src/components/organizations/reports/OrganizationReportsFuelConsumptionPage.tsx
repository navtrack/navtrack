import { TableV2 } from "../../ui/table/TableV2";
import { DistanceReportItemModel } from "@navtrack/shared/api/model";
import { useShow } from "@navtrack/shared/hooks/util/useShow";
import { useRef } from "react";
import { ButtonGroup } from "../../ui/button/ButtonGroup";
import { Card } from "../../ui/card/Card";
import { CustomBarChart } from "../../ui/charts/bar/CustomBarChart";
import { useElementSize } from "@navtrack/shared/hooks/util/useElementSize";
import { useLocationFilter } from "../../asset/shared/location-filter/useLocationFilter";
import { LocationFilter } from "../../asset/shared/location-filter/LocationFilter";
import { useAssetsQuery } from "@navtrack/shared/hooks/queries/assets/useAssetsQuery";
import { useCurrentOrganization } from "@navtrack/shared/hooks/current/useCurrentOrganization";
import { useDistanceReport } from "@navtrack/shared/hooks/reports/useDistanceReport";
import {
  DistanceReportChartItem,
  DistanceReportChartKeys,
  useDistanceReportChart
} from "@navtrack/shared/hooks/reports/useDistanceReportChart";
import { useTable } from "../../ui/table/useTable";

export function OrganizationReportsFuelConsumptionPage() {
  const show = useShow();
  const currentOrganization = useCurrentOrganization();
  const filter = useLocationFilter({
    page: "asset-reports-fuel-consumption",
    filters: []
  });

  const tableRef = useRef<HTMLDivElement>(null);
  const tableSize = useElementSize(tableRef);

  const assets = useAssetsQuery({
    organizationId: currentOrganization.data?.id
  });

  const distanceReport = useDistanceReport({
    assetIds: assets.data?.items.map((asset) => asset.id) || [],
    startDate: filter.filters.startDate,
    endDate: filter.filters.endDate
  });
  const distanceReportChart = useDistanceReportChart({
    items: distanceReport.result.items,
    default: DistanceReportChartKeys.FuelConsumption
  });
  const table = useTable<DistanceReportItemModel>({
    rows: distanceReport.result.items,
    columns: [
      {
        labelId: "date",
        row: (item) => <>{show.date(item.date)}</>,
        value: (item) => item.date,
        sort: "desc"
      },
      {
        labelId: "distance",
        row: (item) => <>{show.distance(item.distance) ?? "-"}</>,
        footer: () => (
          <span className="font-semibold">
            {show.distance(distanceReport.result.totalDistance)}
          </span>
        ),
        value: (item) => item.distance
      },
      {
        labelId: "duration",
        row: (item) => <>{show.duration(item.duration) ?? "-"}</>,
        footer: () => (
          <span className="font-semibold">
            {show.duration(distanceReport.result.totalDuration)}
          </span>
        ),
        value: (item) => item.duration
      },
      {
        labelId: "average-speed",
        row: (item) => <>{show.speed(item.averageSpeed) ?? "-"}</>,
        footer: () => (
          <span className="font-semibold">
            {show.speed(distanceReport.result.averageSpeed)}
          </span>
        ),
        value: (item) => item.averageSpeed
      },
      {
        labelId: "fuel-consumption",
        row: (item) => <>{show.volume(item.fuelConsumption) ?? "-"}</>,
        footer: () => (
          <span className="font-semibold">
            {show.volume(distanceReport.result.totalFuelConsumption) ?? "-"}
          </span>
        ),
        value: (item) => item.fuelConsumption
      },
      {
        labelId: "average-fuel-consumption",
        row: (item) => (
          <>{show.fuelConsumption(item.averageFuelConsumption) ?? "-"}</>
        ),
        footer: () => (
          <span className="font-semibold">
            {show.fuelConsumption(
              distanceReport.result.averageFuelConsumption
            ) ?? "-"}
          </span>
        ),
        value: (item) => item.averageFuelConsumption
      }
    ]
  });

  return (
    <>
      <LocationFilter configuration={filter.configuration} />
      <div className="h-full flex flex-col">
        <div className="flex-1" ref={tableRef}>
          <TableV2<DistanceReportItemModel>
            height={tableSize.height}
            {...table.props}
          />
        </div>
        <Card className="flex-1 p-2 mt-4 flex flex-col">
          <div className="flex justify-center">
            <ButtonGroup
              options={distanceReportChart.fuelConsumptionChartOptions}
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
