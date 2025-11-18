import { LocationFilter } from "../shared/location-filter/LocationFilter";
import { useCurrentAsset } from "@navtrack/shared/hooks/current/useCurrentAsset";
import { TableV2 } from "../../ui/table/TableV2";
import { DistanceReportItemModel } from "@navtrack/shared/api/model";
import { useShow } from "@navtrack/shared/hooks/util/useShow";
import { useAtomValue } from "jotai";
import { locationFiltersSelector } from "../shared/location-filter/locationFilterState";
import { useLocationFilterKey } from "../shared/location-filter/useLocationFilterKey";
import { useAssetReportDistanceQuery } from "@navtrack/shared/hooks/queries/assets/useAssetReportDistanceQuery";
import { Card } from "../../ui/card/Card";
import {
  CustomBarChart,
  CustomBarChartProps
} from "../../ui/charts/bar/CustomBarChart";
import { useElementSize } from "@navtrack/shared/hooks/util/useElementSize";
import { useMemo, useRef, useState } from "react";
import { format } from "date-fns";
import { useConvert } from "@navtrack/shared/hooks/util/useConvert";
import { useCurrentUnits } from "@navtrack/shared/hooks/util/useCurrentUnits";
import { ButtonGroup, ButtonGroupOption } from "../../ui/button/ButtonGroup";

type ChartItem = {
  date: string;
  distance: number;
  duration: number;
  averageSpeed: number;
  maxSpeed: number;
};

const chartOptions: ButtonGroupOption[] = [
  { label: "generic.distance", value: "distance" },
  { label: "generic.duration", value: "duration" },
  { label: "generic.average-speed", value: "average-speed" },
  { label: "generic.max-speed", value: "max-speed" }
];

export function AssetReportsDistancePage() {
  const show = useShow();
  const convert = useConvert();
  const units = useCurrentUnits();
  const currentAsset = useCurrentAsset();
  const locationFilterKey = useLocationFilterKey("reports-distance");
  const filters = useAtomValue(locationFiltersSelector(locationFilterKey));
  const distanceReport = useAssetReportDistanceQuery({
    assetId: currentAsset.data?.id,
    startDate: filters.startDate,
    endDate: filters.endDate
  });

  const tableRef = useRef<HTMLDivElement>(null);
  const tableSize = useElementSize(tableRef);

  const chartItems: ChartItem[] = useMemo(
    () =>
      distanceReport.data?.items.map((item) => ({
        date: format(new Date(item.date), "MMM dd"),
        distance: convert.distance(item.distance) ?? 0,
        duration: convert.durationToHours(item.duration) ?? 0,
        averageSpeed: convert.speed(item.averageSpeed) ?? 0,
        maxSpeed: convert.speed(item.maxSpeed) ?? 0
      })) ?? [],
    [distanceReport.data?.items, convert]
  );

  const distanceChartConfig: CustomBarChartProps<ChartItem> = {
    bars: [
      {
        key: "distance",
        labelId: "generic.distance"
      }
    ],
    xAxis: { dataKey: "date" },
    yAxis: { dataKey: "distance", unit: units.lengthK }
  };

  const durationChartConfig: CustomBarChartProps<ChartItem> = {
    bars: [
      {
        key: "duration",
        labelId: "generic.duration"
      }
    ],
    xAxis: { dataKey: "date" },
    yAxis: {
      dataKey: "duration",
      unit: "h"
    }
  };

  const averageSpeedChartConfig: CustomBarChartProps<ChartItem> = {
    bars: [
      {
        key: "averageSpeed",
        labelId: "generic.average-speed"
      }
    ],
    xAxis: { dataKey: "date" },
    yAxis: { dataKey: "averageSpeed", unit: units.speed }
  };

  const maxSpeedChartConfig: CustomBarChartProps<ChartItem> = {
    bars: [
      {
        key: "maxSpeed",
        labelId: "generic.max-speed"
      }
    ],
    xAxis: { dataKey: "date" },
    yAxis: { dataKey: "maxSpeed", unit: units.speed }
  };

  const [chartConfig, setChartConfig] =
    useState<CustomBarChartProps<ChartItem>>(distanceChartConfig);

  const onChartTypeChange = (value: string) => {
    switch (value) {
      case "distance":
        setChartConfig(distanceChartConfig);
        break;
      case "duration":
        setChartConfig(durationChartConfig);
        break;
      case "average-speed":
        setChartConfig(averageSpeedChartConfig);
        break;
      case "max-speed":
        setChartConfig(maxSpeedChartConfig);
        break;
    }
  };

  return (
    <>
      <LocationFilter filterPage="reports-distance" />
      <div className="h-full flex flex-col">
        <div className="flex-1" ref={tableRef}>
          <TableV2<DistanceReportItemModel>
            height={tableSize.height}
            columns={[
              {
                labelId: "generic.date",
                row: (item) => <>{show.date(item.date)}</>,
                footer: () => <></>,
                sortValue: (item) => item.date,
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
                sortValue: (item) => item.distance
              },
              {
                labelId: "generic.duration",
                row: (item) => <>{show.duration(item.duration) ?? "-"}</>,
                footer: () => (
                  <span className="font-semibold">
                    {show.duration(distanceReport.data?.totalDuration)}
                  </span>
                ),
                sortValue: (item) => item.duration
              },
              {
                labelId: "generic.average-speed",
                row: (item) => <>{show.speed(item.averageSpeed) ?? "-"}</>,
                footer: () => (
                  <span className="font-semibold">
                    {show.speed(distanceReport.data?.averageSpeed)}
                  </span>
                ),
                sortValue: (item) => item.averageSpeed
              },
              {
                labelId: "generic.max-speed",
                row: (item) => <>{show.speed(item.maxSpeed) ?? "-"}</>,
                footer: () => (
                  <span className="font-semibold">
                    {show.speed(distanceReport.data?.maxSpeed)}
                  </span>
                ),
                sortValue: (item) => item.maxSpeed
              }
            ]}
            rows={distanceReport.data?.items}
          />
        </div>
        <Card className="flex-1 p-2 mt-4 flex flex-col">
          <div className="flex justify-center">
            <ButtonGroup options={chartOptions} onChange={onChartTypeChange} />
          </div>
          <div className="mt-4 flex-1">
            <CustomBarChart<ChartItem>
              {...chartConfig}
              data={chartItems}
              hideLegend
            />
          </div>
        </Card>
      </div>
    </>
  );
}
