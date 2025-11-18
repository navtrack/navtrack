import { differenceInSeconds } from "date-fns";
import { LatLong } from "../../../api/model";
import { useMemo } from "react";
import { useAssetTripsQueries } from "./useAssetTripsQueries";

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

export type TripStopModel = {
  arrivalDate: string;
  arrivalCoordinates: LatLong;
  departureDate: string;
  departureCoordinates?: LatLong;
  duration?: number;
};

export function useAssetTripsStopsQueries(props: AssetTripsQueriesProps) {
  const tripsQuery = useAssetTripsQueries(props);

  const tripStops = useMemo(() => {
    const ordered = tripsQuery.items.sort((a, b) =>
      a.startPosition.date < b.startPosition.date ? -1 : 1
    );

    const stops: TripStopModel[] = [];

    ordered.forEach((trip, index) => {
      const current = trip;
      const next = ordered.length > index + 1 ? ordered[index + 1] : undefined;

      const stop: TripStopModel = {
        arrivalDate: current.endPosition.date,
        departureDate: next?.startPosition.date ?? new Date().toISOString(),
        arrivalCoordinates: current.endPosition.coordinates,
        departureCoordinates: next?.endPosition.coordinates
      };

      stop.duration = differenceInSeconds(stop.departureDate, stop.arrivalDate);

      stops.push(stop);
    });

    return stops.toReversed();
  }, [tripsQuery.items]);

  return tripStops;
}
