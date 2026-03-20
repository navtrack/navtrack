import { CustomBarChartProps } from "../../../../web/src/components/ui/charts/bar/CustomBarChart";
import { useCallback, useMemo, useState } from "react";
import { format } from "date-fns";
import { ButtonGroupOption } from "../../../../web/src/components/ui/button/ButtonGroup";
import { DistanceReportItemModel } from "../../api/model";
import { useConvert } from "../util/useConvert";
import { useCurrentUnits } from "../util/useCurrentUnits";

export type DistanceReportChartItem = {
  date: string;
  distance: number;
  duration: number;
  averageSpeed: number;
  maxSpeed: number;
  fuelConsumption: number;
  averageFuelConsumption: number;
};

type DistanceReportChartProps = {
  items?: DistanceReportItemModel[];
};

const enum ChartKeys {
  Distance = "distance",
  Duration = "duration",
  AverageSpeed = "averageSpeed",
  MaxSpeed = "maxSpeed",
  FuelConsumption = "fuelConsumption",
  AverageFuelConsumption = "averageFuelConsumption"
}

export function useDistanceReportChart(props: DistanceReportChartProps) {
  const convert = useConvert();
  const units = useCurrentUnits();

  const chartItems: DistanceReportChartItem[] = useMemo(
    () =>
      (props.items ?? []).map((item) => ({
        date: format(new Date(item.date), "MMM dd"),
        distance: convert.distance(item.distance) ?? 0,
        duration: convert.durationToHours(item.duration) ?? 0,
        averageSpeed: convert.speed(item.averageSpeed) ?? 0,
        maxSpeed: convert.speed(item.maxSpeed) ?? 0,
        fuelConsumption: convert.fuelConsumption(item.fuelConsumption) ?? 0,
        averageFuelConsumption:
          convert.fuelConsumption(item.averageFuelConsumption) ?? 0
      })) ?? [],
    [props.items, convert]
  );

  const distanceChartConfig: CustomBarChartProps<DistanceReportChartItem> =
    useMemo(
      () => ({
        bars: [
          {
            key: ChartKeys.Distance,
            labelId: "generic.distance"
          }
        ],
        xAxis: { dataKey: "date" },
        yAxis: { dataKey: ChartKeys.Distance, unit: units.lengthK }
      }),
      [units.lengthK]
    );

  const durationChartConfig: CustomBarChartProps<DistanceReportChartItem> =
    useMemo(
      () => ({
        bars: [
          {
            key: ChartKeys.Duration,
            labelId: "generic.duration"
          }
        ],
        xAxis: { dataKey: "date" },
        yAxis: {
          dataKey: ChartKeys.Duration,
          unit: "h"
        }
      }),
      []
    );

  const averageSpeedChartConfig: CustomBarChartProps<DistanceReportChartItem> =
    useMemo(
      () => ({
        bars: [
          {
            key: ChartKeys.AverageSpeed,
            labelId: "generic.average-speed"
          }
        ],
        xAxis: { dataKey: "date" },
        yAxis: { dataKey: ChartKeys.AverageSpeed, unit: units.speed }
      }),
      [units.speed]
    );

  const maxSpeedChartConfig: CustomBarChartProps<DistanceReportChartItem> =
    useMemo(
      () => ({
        bars: [
          {
            key: ChartKeys.MaxSpeed,
            labelId: "generic.max-speed"
          }
        ],
        xAxis: { dataKey: "date" },
        yAxis: { dataKey: ChartKeys.MaxSpeed, unit: units.speed }
      }),
      [units.speed]
    );

  const fuelConsumptionChartConfig: CustomBarChartProps<DistanceReportChartItem> =
    useMemo(
      () => ({
        bars: [
          {
            key: ChartKeys.FuelConsumption,
            labelId: "generic.fuel-consumption"
          }
        ],
        xAxis: { dataKey: "date" },
        yAxis: { dataKey: ChartKeys.FuelConsumption, unit: units.volume }
      }),
      [units.volume]
    );

  const averageFuelConsumptionChartConfig: CustomBarChartProps<DistanceReportChartItem> =
    useMemo(
      () => ({
        bars: [
          {
            key: ChartKeys.AverageFuelConsumption,
            labelId: "generic.average-fuel-consumption"
          }
        ],
        xAxis: { dataKey: "date" },
        yAxis: {
          dataKey: ChartKeys.AverageFuelConsumption,
          unit: units.fuelConsumption
        }
      }),
      [units.fuelConsumption]
    );

  const [chartConfig, setChartConfig] =
    useState<CustomBarChartProps<DistanceReportChartItem>>(distanceChartConfig);

  const onChartTypeChange = useCallback(
    (value: string) => {
      switch (value) {
        case ChartKeys.Distance:
          setChartConfig(distanceChartConfig);
          break;
        case ChartKeys.Duration:
          setChartConfig(durationChartConfig);
          break;
        case ChartKeys.AverageSpeed:
          setChartConfig(averageSpeedChartConfig);
          break;
        case ChartKeys.MaxSpeed:
          setChartConfig(maxSpeedChartConfig);
          break;
        case ChartKeys.FuelConsumption:
          setChartConfig(fuelConsumptionChartConfig);
          break;
        case ChartKeys.AverageFuelConsumption:
          setChartConfig(averageFuelConsumptionChartConfig);
          break;
      }
    },
    [
      averageFuelConsumptionChartConfig,
      averageSpeedChartConfig,
      distanceChartConfig,
      durationChartConfig,
      fuelConsumptionChartConfig,
      maxSpeedChartConfig
    ]
  );

  const distanceChartOptions: ButtonGroupOption[] = [
    { label: "generic.distance", value: ChartKeys.Distance },
    { label: "generic.duration", value: ChartKeys.Duration },
    { label: "generic.average-speed", value: ChartKeys.AverageSpeed },
    { label: "generic.max-speed", value: ChartKeys.MaxSpeed }
  ];

  const fuelConsumptionChartOptions: ButtonGroupOption[] = [
    { label: "generic.fuel-consumption", value: ChartKeys.FuelConsumption },
    {
      label: "generic.average-fuel-consumption",
      value: ChartKeys.AverageFuelConsumption
    },
    { label: "generic.distance", value: ChartKeys.Distance },
    { label: "generic.duration", value: ChartKeys.Duration }
  ];

  return {
    chartItems,
    chartConfig,
    distanceChartOptions,
    fuelConsumptionChartOptions,
    onChartTypeChange
  };
}
