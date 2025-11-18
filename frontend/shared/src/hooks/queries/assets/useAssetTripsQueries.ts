import { useQueries } from "@tanstack/react-query";
import {
  assetsTripsGetList,
  getAssetsTripsGetListQueryKey
} from "../../../api";
import { eachDayOfInterval } from "date-fns";
import { formatApiDate } from "../../../utils/api";
import { useMemo } from "react";

export type AssetTripsQueriesProps = {
  assetId?: string;
  startDate?: string;
  endDate?: string;
  minAltitude?: number;
  maxAltitude?: number;
  minDuration?: number;
  maxDuration?: number;
  minSpeed?: number;
  maxSpeed?: number;
  latitude?: number;
  longitude?: number;
  radius?: number;
};

export function useAssetTripsQueries(props: AssetTripsQueriesProps) {
  const days = eachDayOfInterval({
    start: props.startDate ?? new Date(),
    end: props.endDate ?? new Date()
  });

  const queries = useQueries({
    queries: days.map((day) => ({
      queryKey: getAssetsTripsGetListQueryKey(props.assetId!, {
        Date: formatApiDate(day),
        MinAvgSpeed: props.minSpeed,
        MaxAvgSpeed: props.maxSpeed,
        MinAltitude: props.minAltitude,
        MaxAltitude: props.maxAltitude,
        MinDuration: props.minDuration,
        MaxDuration: props.maxDuration,
        Latitude: props.latitude,
        Longitude: props.longitude,
        Radius: props.radius
      }),
      queryFn: (q) =>
        assetsTripsGetList(
          props.assetId!,
          {
            Date: formatApiDate(day),
            MinAvgSpeed: props.minSpeed,
            MaxAvgSpeed: props.maxSpeed,
            MinAltitude: props.minAltitude,
            MaxAltitude: props.maxAltitude,
            MinDuration: props.minDuration,
            MaxDuration: props.maxDuration,
            Latitude: props.latitude,
            Longitude: props.longitude,
            Radius: props.radius
          },
          q.signal
        ),
      staleTime: Infinity
    }))
  });

  const result = useMemo(
    () => ({
      queries,
      items: queries.flatMap((q) => q.data?.items || []),
      isLoading: queries.some((q) => q.isLoading),
      totalTrips: queries.reduce(
        (sum, q) => sum + (q.data?.items.length || 0),
        0
      ),
      totalDuration: queries.reduce(
        (sum, q) => sum + (q.data?.totalDuration || 0),
        0
      ),
      totalDistance: queries.reduce(
        (sum, q) => sum + (q.data?.totalDistance || 0),
        0
      ),
      averageSpeed:
        queries.reduce((sum, q) => sum + (q.data?.averageSpeed || 0), 0) /
        (queries.filter((q) => q.data?.averageSpeed !== undefined).length || 1),
      maxSpeed: Math.max(...queries.map((q) => q.data?.maxSpeed || 0)),
      averageFuelConsumption:
        queries.reduce(
          (sum, q) => sum + (q.data?.averageFuelConsumption || 0),
          0
        ) /
        (queries.filter((q) => q.data?.averageFuelConsumption !== undefined)
          .length || 1),
      totalFuelConsumption: queries.reduce(
        (sum, q) => sum + (q.data?.fuelConsumption || 0),
        0
      )
    }),
    [queries]
  );

  return result;
}
