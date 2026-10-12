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
  default?: DistanceReportChartKeys;
};

export const enum DistanceReportChartKeys {
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
            key: DistanceReportChartKeys.Distance,
            labelId: "distance"
          }
        ],
        xAxis: { dataKey: "date" },
        yAxis: {
          dataKey: DistanceReportChartKeys.Distance,
          unit: units.lengthK
        }
      }),
      [units.lengthK]
    );

  const durationChartConfig: CustomBarChartProps<DistanceReportChartItem> =
    useMemo(
      () => ({
        bars: [
          {
            key: DistanceReportChartKeys.Duration,
            labelId: "duration"
          }
        ],
        xAxis: { dataKey: "date" },
        yAxis: {
          dataKey: DistanceReportChartKeys.Duration,
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
            key: DistanceReportChartKeys.AverageSpeed,
            labelId: "average-speed"
          }
        ],
        xAxis: { dataKey: "date" },
        yAxis: {
          dataKey: DistanceReportChartKeys.AverageSpeed,
          unit: units.speed
        }
      }),
      [units.speed]
    );

  const maxSpeedChartConfig: CustomBarChartProps<DistanceReportChartItem> =
    useMemo(
      () => ({
        bars: [
          {
            key: DistanceReportChartKeys.MaxSpeed,
            labelId: "max-speed"
          }
        ],
        xAxis: { dataKey: "date" },
        yAxis: { dataKey: DistanceReportChartKeys.MaxSpeed, unit: units.speed }
      }),
      [units.speed]
    );

  const fuelConsumptionChartConfig: CustomBarChartProps<DistanceReportChartItem> =
    useMemo(
      () => ({
        bars: [
          {
            key: DistanceReportChartKeys.FuelConsumption,
            labelId: "fuel-consumption"
          }
        ],
        xAxis: { dataKey: "date" },
        yAxis: {
          dataKey: DistanceReportChartKeys.FuelConsumption,
          unit: units.volume
        }
      }),
      [units.volume]
    );

  const averageFuelConsumptionChartConfig: CustomBarChartProps<DistanceReportChartItem> =
    useMemo(
      () => ({
        bars: [
          {
            key: DistanceReportChartKeys.AverageFuelConsumption,
            labelId: "average-fuel-consumption"
          }
        ],
        xAxis: { dataKey: "date" },
        yAxis: {
          dataKey: DistanceReportChartKeys.AverageFuelConsumption,
          unit: units.fuelConsumption
        }
      }),
      [units.fuelConsumption]
    );

  const chartConfigs = useMemo(
    () => ({
      [DistanceReportChartKeys.Distance]: distanceChartConfig,
      [DistanceReportChartKeys.Duration]: durationChartConfig,
      [DistanceReportChartKeys.AverageSpeed]: averageSpeedChartConfig,
      [DistanceReportChartKeys.MaxSpeed]: maxSpeedChartConfig,
      [DistanceReportChartKeys.FuelConsumption]: fuelConsumptionChartConfig,
      [DistanceReportChartKeys.AverageFuelConsumption]:
        averageFuelConsumptionChartConfig
    }),
    [
      averageFuelConsumptionChartConfig,
      averageSpeedChartConfig,
      distanceChartConfig,
      durationChartConfig,
      fuelConsumptionChartConfig,
      maxSpeedChartConfig
    ]
  );

  const [chartConfig, setChartConfig] = useState<
    CustomBarChartProps<DistanceReportChartItem>
  >(chartConfigs[props.default ?? DistanceReportChartKeys.Distance]);

  const onChartTypeChange = useCallback(
    (value: string) => {
      setChartConfig(
        chartConfigs[value as DistanceReportChartKeys] ?? distanceChartConfig
      );
    },
    [chartConfigs, distanceChartConfig]
  );

  const distanceChartOptions: ButtonGroupOption[] = [
    { label: "distance", value: DistanceReportChartKeys.Distance },
    { label: "duration", value: DistanceReportChartKeys.Duration },
    {
      label: "average-speed",
      value: DistanceReportChartKeys.AverageSpeed
    },
    { label: "max-speed", value: DistanceReportChartKeys.MaxSpeed }
  ];

  const fuelConsumptionChartOptions: ButtonGroupOption[] = [
    {
      label: "fuel-consumption",
      value: DistanceReportChartKeys.FuelConsumption
    },
    {
      label: "average-fuel-consumption",
      value: DistanceReportChartKeys.AverageFuelConsumption
    },
    { label: "distance", value: DistanceReportChartKeys.Distance },
    { label: "duration", value: DistanceReportChartKeys.Duration }
  ];

  return {
    chartItems,
    chartConfig,
    distanceChartOptions,
    fuelConsumptionChartOptions,
    onChartTypeChange
  };
}
