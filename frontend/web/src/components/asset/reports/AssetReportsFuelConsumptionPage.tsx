import { useAssetReportFuelConsumptionQuery } from "@navtrack/shared/hooks/queries/assets/useAssetReportFuelConsumptionQuery";
import { LocationFilter } from "../shared/location-filter/LocationFilter";
import { useCurrentAsset } from "@navtrack/shared/hooks/current/useCurrentAsset";
import { TableV2 } from "../../ui/table/TableV2";
import { FuelConsumptionReportItemModel } from "@navtrack/shared/api/model";
import { useShow } from "@navtrack/shared/hooks/util/useShow";
import { FormattedMessage } from "react-intl";
import { useAtomValue } from "jotai";
import { locationFiltersSelector } from "../shared/location-filter/locationFilterState";
import { useLocationFilterKey } from "../shared/location-filter/useLocationFilterKey";
import { useMemo, useRef, useState } from "react";
import { ButtonGroup, ButtonGroupOption } from "../../ui/button/ButtonGroup";
import { Card } from "../../ui/card/Card";
import {
  CustomBarChartProps,
  CustomBarChart
} from "../../ui/charts/bar/CustomBarChart";
import { useCurrentUnits } from "@navtrack/shared/hooks/util/useCurrentUnits";
import { useElementSize } from "@navtrack/shared/hooks/util/useElementSize";
import { format } from "date-fns";
import { useConvert } from "@navtrack/shared/hooks/util/useConvert";

type ChartItem = {
  date: string;
  distance: number;
  duration: number;
  fuelConsumption: number;
  avgFuelConsumption: number;
};

const chartOptions: ButtonGroupOption[] = [
  { label: "generic.fuel-consumption", value: "fuelConsumption" },
  { label: "generic.average-fuel-consumption", value: "avgFuelConsumption" },
  { label: "generic.distance", value: "distance" },
  { label: "generic.duration", value: "duration" }
];

export function AssetReportsFuelConsumptionPage() {
  const show = useShow();
  const units = useCurrentUnits();
  const convert = useConvert();
  const currentAsset = useCurrentAsset();
  const locationFilterKey = useLocationFilterKey("reports-fuel-consumption");
  const filters = useAtomValue(locationFiltersSelector(locationFilterKey));
  const fuelConsumptionReport = useAssetReportFuelConsumptionQuery({
    assetId: currentAsset.data?.id,
    startDate: filters.startDate,
    endDate: filters.endDate
  });

  const tableRef = useRef<HTMLDivElement>(null);
  const tableSize = useElementSize(tableRef);

  const chartItems: ChartItem[] = useMemo(
    () =>
      fuelConsumptionReport.data?.items.map((item) => ({
        date: format(new Date(item.date), "MMM dd"),
        distance: convert.distance(item.distance) ?? 0,
        duration: convert.durationToHours(item.duration) ?? 0,
        fuelConsumption: convert.fuelConsumption(item.fuelConsumption) ?? 0,
        avgFuelConsumption:
          convert.fuelConsumption(item.averageFuelConsumption) ?? 0
      })) ?? [],
    [fuelConsumptionReport.data?.items, convert]
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
        key: "fuelConsumption",
        labelId: "generic.fuel-consumption"
      }
    ],
    xAxis: { dataKey: "date" },
    yAxis: { dataKey: "fuelConsumption", unit: units.volume }
  };

  const maxSpeedChartConfig: CustomBarChartProps<ChartItem> = {
    bars: [
      {
        key: "avgFuelConsumption",
        labelId: "generic.average-fuel-consumption"
      }
    ],
    xAxis: { dataKey: "date" },
    yAxis: { dataKey: "avgFuelConsumption", unit: units.fuelConsumption }
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
      case "fuelConsumption":
        setChartConfig(averageSpeedChartConfig);
        break;
      case "avgFuelConsumption":
        setChartConfig(maxSpeedChartConfig);
        break;
    }
  };

  return (
    <>
      <LocationFilter filterPage="reports-fuel-consumption" />
      <div className="h-full flex flex-col">
        <div className="flex-1" ref={tableRef}>
          <TableV2<FuelConsumptionReportItemModel>
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
                    {show.distance(fuelConsumptionReport.data?.totalDistance)}
                  </span>
                ),
                sortValue: (item) => item.distance
              },
              {
                labelId: "generic.duration",
                row: (item) => <>{show.duration(item.duration) ?? "-"}</>,
                footer: () => (
                  <span className="font-semibold">
                    {show.duration(fuelConsumptionReport.data?.totalDuration)}
                  </span>
                ),
                sortValue: (item) => item.duration
              },
              {
                labelId: "generic.average-speed",
                row: (item) => <>{show.speed(item.averageSpeed) ?? "-"}</>,
                footer: () => (
                  <span className="font-semibold">
                    {show.speed(fuelConsumptionReport.data?.averageSpeed)}
                  </span>
                ),
                sortValue: (item) => item.averageSpeed
              },
              {
                labelId: "generic.fuel-consumption",
                row: (item) => <>{show.volume(item.fuelConsumption) ?? "-"}</>,
                footer: () => (
                  <span className="font-semibold">
                    {show.volume(
                      fuelConsumptionReport.data?.totalFuelConsumption
                    )}
                  </span>
                ),
                sortValue: (item) => item.fuelConsumption
              },
              {
                labelId: "generic.average-fuel-consumption",
                row: (item) => (
                  <>
                    {show.fuelConsumption(item.averageFuelConsumption) ?? "-"}
                  </>
                ),
                footer: () => (
                  <span className="font-semibold">
                    {show.fuelConsumption(
                      fuelConsumptionReport.data?.averageFuelConsumption
                    )}
                  </span>
                ),
                sortValue: (item) => item.averageFuelConsumption
              }
            ]}
            rows={fuelConsumptionReport.data?.items}
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
