import { useMemo, useState } from "react";
import { useAssetReportDistanceQueries } from "../queries/assets/useAssetReportDistanceQueries";
import { DistanceReportItemModel } from "../../api/model";
import { isNumeric } from "../../utils/numbers";
import { getWeek, parseISO } from "date-fns";

type DistanceReportProps = {
  assetIds?: string[];
  startDate?: string;
  endDate?: string;
};

export type DistanceReportResult = {
  items: DistanceReportItem[] | undefined;
  totalDistance: number;
  totalDuration: number;
  averageSpeed: number;
  maxSpeed: number;
  totalFuelConsumption?: number;
  averageFuelConsumption?: number;
};

export type DistanceReport = {
  result: DistanceReportResult;
  groupBy: GroupBy;
  setGroupBy: (groupBy: GroupBy) => void;
};

type DistanceReportItem = DistanceReportItemModel & {
  key: string;
};

export enum GroupBy {
  Day = "Day",
  Week = "Week",
  Month = "Month",
  Year = "Year"
}

export const groupByLabels: Record<GroupBy, string> = {
  [GroupBy.Day]: "generic.day",
  [GroupBy.Week]: "generic.week",
  [GroupBy.Month]: "generic.month",
  [GroupBy.Year]: "generic.year"
};

function getGroupByDateKey(group: GroupBy) {
  if (group === GroupBy.Day) {
    return (d: Date) => `y${d.getFullYear()}m${d.getMonth()}w${d.getDate()}`;
  } else if (group === GroupBy.Week) {
    return (d: Date) =>
      `y${d.getFullYear()}w${getWeek(d, { weekStartsOn: 1 })}`;
  } else if (group === GroupBy.Month) {
    return (d: Date) => `y${d.getFullYear()}m${d.getMonth()}`;
  }

  return (d: Date) => `y${d.getFullYear()}`;
}

export function useDistanceReport(props: DistanceReportProps): DistanceReport {
  const [groupBy, setGroupBy] = useState(GroupBy.Day);

  const distanceQueries = useAssetReportDistanceQueries({
    assetIds: props.assetIds,
    startDate: props.startDate,
    endDate: props.endDate
  });

  const result: DistanceReportResult = useMemo(() => {
    const loading = distanceQueries.every((query) => query.isLoading);

    if (loading) {
      return {
        items: undefined,
        totalDistance: 0,
        totalDuration: 0,
        averageSpeed: 0,
        maxSpeed: 0
      };
    }

    const mergedItems: DistanceReportItem[] = [];
    let totalDistance = 0;
    let totalDuration = 0;
    let averageSpeed = 0;
    let maxSpeed = 0;
    let totalFuelConsumption: number | undefined = undefined;
    let averageFuelConsumption: number | undefined = undefined;

    distanceQueries.forEach((query) => {
      if (query.data?.items) {
        query.data?.items.forEach((item) => {
          const existingItem = mergedItems.find(
            (mergedItem) =>
              getGroupByDateKey(groupBy)(parseISO(mergedItem.date)) ===
              getGroupByDateKey(groupBy)(parseISO(item.date))
          );

          if (existingItem) {
            existingItem.distance += item.distance;
            existingItem.duration += item.duration;
            existingItem.averageSpeed =
              (existingItem.averageSpeed + item.averageSpeed) / 2;
            existingItem.maxSpeed = Math.max(
              existingItem.maxSpeed,
              item.maxSpeed
            );
            existingItem.fuelConsumption =
              (existingItem.fuelConsumption ?? 0) + (item.fuelConsumption ?? 0);
            existingItem.averageFuelConsumption =
              ((existingItem.averageFuelConsumption ?? 0) +
                (item.averageFuelConsumption ?? 0)) /
              2;
          } else {
            mergedItems.push({
              ...item,
              key: getGroupByDateKey(groupBy)(parseISO(item.date))
            });
          }

          totalDistance += item.distance;
          totalDuration += item.duration;
          averageSpeed =
            item.averageSpeed > 0
              ? (averageSpeed + item.averageSpeed) / 2
              : averageSpeed;

          const hasFuelConsumption = isNumeric(item.fuelConsumption);
          totalFuelConsumption = hasFuelConsumption
            ? (totalFuelConsumption ?? 0) + (item.fuelConsumption ?? 0)
            : totalFuelConsumption;
          const hasAverageFuelConsumption = isNumeric(
            item.averageFuelConsumption
          );
          averageFuelConsumption = hasAverageFuelConsumption
            ? isNumeric(averageFuelConsumption)
              ? ((averageFuelConsumption ?? 0) +
                  (item.averageFuelConsumption ?? 0)) /
                2
              : (item.averageFuelConsumption ?? 0)
            : averageFuelConsumption;
        });
      }
    });

    mergedItems.sort(
      (a, b) => new Date(b.date).getTime() - new Date(a.date).getTime()
    );

    return {
      items: mergedItems,
      totalDistance,
      totalDuration,
      averageSpeed,
      maxSpeed,
      totalFuelConsumption,
      averageFuelConsumption
    };
  }, [distanceQueries, groupBy]);

  return {
    result,
    groupBy,
    setGroupBy
  };
}
