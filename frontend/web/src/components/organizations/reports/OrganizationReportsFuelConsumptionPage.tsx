import { useAssetReportFuelConsumptionQuery } from "@navtrack/shared/hooks/queries/assets/useAssetReportFuelConsumptionQuery";
import { useCurrentAsset } from "@navtrack/shared/hooks/current/useCurrentAsset";
import { TableV2 } from "../../ui/table/TableV2";
import { FuelConsumptionReportItemModel } from "@navtrack/shared/api/model";
import { useShow } from "@navtrack/shared/hooks/util/useShow";
import { useAtomValue } from "jotai";
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
import { useLocationFilterKey } from "../../asset/shared/location-filter/useLocationFilterKey";
import { locationFiltersSelector } from "../../asset/shared/location-filter/locationFilterState";
import { LocationFilter } from "../../asset/shared/location-filter/LocationFilter";
import { useAssetFuelConsumptionReportsQueries } from "@navtrack/shared/hooks/queries/assets/useAssetFuelConsumptionReportsQueries";
import { useAssetsQuery } from "@navtrack/shared/hooks/queries/assets/useAssetsQuery";
import { useCurrentOrganization } from "@navtrack/shared/hooks/current/useCurrentOrganization";

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

export function OrganizationReportsFuelConsumptionPage() {
  const show = useShow();
  const units = useCurrentUnits();
  const convert = useConvert();
  const currentOrganization = useCurrentOrganization();
  const locationFilterKey = useLocationFilterKey("reports-fuel-consumption");
  const filters = useAtomValue(locationFiltersSelector(locationFilterKey));

  const assets = useAssetsQuery({
    organizationId: currentOrganization.data?.id
  });
  const distanceQueries = useAssetFuelConsumptionReportsQueries({
    assetIds: assets.data?.items.map((asset) => asset.id) || [],
    startDate: filters.startDate,
    endDate: filters.endDate
  });

  const items: FuelConsumptionReportItemModel[] = useMemo(() => {
    const mergedItems: FuelConsumptionReportItemModel[] = [];

    distanceQueries.forEach((query) => {
      if (query.data?.items) {
        query.data?.items.forEach((item) => {
          // Check if the item for the same date already exists
          const existingItem = mergedItems.find(
            (mergedItem) => mergedItem.date === item.date
          );

          if (existingItem) {
            // If it exists, aggregate the values
            existingItem.distance += item.distance;
            existingItem.duration += item.duration;
            existingItem.averageSpeed =
              ((existingItem.averageSpeed ?? 0) + (item.averageSpeed ?? 0)) / 2;
            existingItem.fuelConsumption =
              (existingItem.fuelConsumption ?? 0) + (item.fuelConsumption ?? 0);
            existingItem.averageFuelConsumption =
              ((existingItem.averageFuelConsumption ?? 0) +
                (item.averageFuelConsumption ?? 0)) /
              2;
          } else {
            // If it doesn't exist, add a new entry
            mergedItems.push({ ...item });
          }
        });
      }
    });

    // Optionally, sort the merged items by date
    mergedItems.sort(
      (a, b) => new Date(a.date).getTime() - new Date(b.date).getTime()
    );

    return mergedItems;
  }, [distanceQueries]);

  const tableRef = useRef<HTMLDivElement>(null);
  const tableSize = useElementSize(tableRef);

  const chartItems: ChartItem[] = useMemo(
    () =>
      items.map((item) => ({
        date: format(new Date(item.date), "MMM dd"),
        distance: convert.distance(item.distance) ?? 0,
        duration: convert.durationToHours(item.duration) ?? 0,
        fuelConsumption: convert.fuelConsumption(item.fuelConsumption) ?? 0,
        avgFuelConsumption:
          convert.fuelConsumption(item.averageFuelConsumption) ?? 0
      })) ?? [],
    [items, convert]
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
            rows={items}
            height={tableSize.height}
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
                    {/* {show.distance(fuelConsumptionReport.data?.totalDistance)} */}
                  </span>
                ),
                value: (item) => item.distance
              },
              {
                labelId: "generic.duration",
                row: (item) => <>{show.duration(item.duration) ?? "-"}</>,
                footer: () => (
                  <span className="font-semibold">
                    {/* {show.duration(fuelConsumptionReport.data?.totalDuration)} */}
                  </span>
                ),
                value: (item) => item.duration
              },
              {
                labelId: "generic.average-speed",
                row: (item) => <>{show.speed(item.averageSpeed) ?? "-"}</>,
                footer: () => (
                  <span className="font-semibold">
                    {/* {show.speed(fuelConsumptionReport.data?.averageSpeed)} */}
                  </span>
                ),
                value: (item) => item.averageSpeed
              },
              {
                labelId: "generic.fuel-consumption",
                row: (item) => <>{show.volume(item.fuelConsumption) ?? "-"}</>,
                footer: () => (
                  <span className="font-semibold">
                    {/* {show.volume(
                      fuelConsumptionReport.data?.totalFuelConsumption
                    )} */}
                  </span>
                ),
                value: (item) => item.fuelConsumption
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
                    {/* {show.fuelConsumption(
                      fuelConsumptionReport.data?.averageFuelConsumption
                    )} */}
                  </span>
                ),
                value: (item) => item.averageFuelConsumption
              }
            ]}
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
