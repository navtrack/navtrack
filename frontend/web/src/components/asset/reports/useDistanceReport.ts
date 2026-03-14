import { useMemo } from "react";
import { useAssetReportDistanceQueries } from "@navtrack/shared/hooks/queries/assets/useAssetReportDistanceQueries";
import { DistanceReportItemModel } from "@navtrack/shared/api/model";

type DistanceReportProps = {
  assetIds: string[];
  startDate?: string;
  endDate?: string;
};

export function useDistanceReport(props: DistanceReportProps) {
  const distanceQueries = useAssetReportDistanceQueries({
    assetIds: props.assetIds,
    startDate: props.startDate,
    endDate: props.endDate
  });

  const result = useMemo(() => {
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

    const mergedItems: DistanceReportItemModel[] = [];
    let totalDistance = 0;
    let totalDuration = 0;
    let averageSpeed = 0;
    let maxSpeed = 0;
    let totalFuelConsumption = 0;
    let averageFuelConsumption = 0;

    distanceQueries.forEach((query) => {
      if (query.data?.items) {
        query.data?.items.forEach((item) => {
          const existingItem = mergedItems.find(
            (mergedItem) => mergedItem.date === item.date
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
            mergedItems.push(item);
          }

          totalDistance += item.distance;
          totalDuration += item.duration;
          averageSpeed =
            item.averageSpeed > 0
              ? (averageSpeed + item.averageSpeed) / 2
              : averageSpeed;
          maxSpeed = Math.max(maxSpeed, item.maxSpeed);
          totalFuelConsumption += item.fuelConsumption ?? 0;
          averageFuelConsumption =
            (item.averageFuelConsumption ?? 0) > 0
              ? (averageFuelConsumption + (item.averageFuelConsumption ?? 0)) /
                2
              : averageFuelConsumption;
        });
      }
    });

    mergedItems.sort(
      (a, b) => new Date(a.date).getTime() - new Date(b.date).getTime()
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
  }, [distanceQueries]);

  return {
    data: result.items,
    totalDistance: result.totalDistance,
    totalDuration: result.totalDuration,
    averageSpeed: result.averageSpeed,
    maxSpeed: result.maxSpeed,
    totalFuelConsumption: result.totalFuelConsumption,
    averageFuelConsumption: result.averageFuelConsumption
  };
}
