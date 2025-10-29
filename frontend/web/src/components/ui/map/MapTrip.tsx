import L, { LatLng, Polyline as LeafletPolyline } from "leaflet";
import { useEffect, useState } from "react";
import { useMap } from "./useMap";
import { TripModel } from "@navtrack/shared/api/model";
import { Polyline } from "react-leaflet";
import { MapOptionsDto } from "@navtrack/shared/maps";
import { MapPin } from "./MapPin";

type MapTripProps = { trip?: TripModel; options?: MapOptionsDto };

export function MapTrip(props: MapTripProps) {
  const map = useMap();
  const [polyline, setPolyline] = useState<LeafletPolyline | undefined>(
    undefined
  );
  const [currentTrip, setCurrentTrip] = useState<TripModel | undefined>(
    undefined
  );

  useEffect(() => {
    if (props.trip !== undefined && props.trip !== currentTrip) {
      setCurrentTrip(props.trip);

      const latlngs = props.trip.positions.map(
        (x) => new LatLng(x.coordinates.latitude, x.coordinates.longitude)
      );

      const leafletPolyline = L.polyline(latlngs);
      setPolyline(leafletPolyline);

      map.fitBounds(
        props.trip.positions.map((x) => x.coordinates),
        props.options
      );
    }

    return () => {
      if (polyline !== undefined) {
        setPolyline(undefined);
      }
    };
  }, [currentTrip, map, map.leafletMap, polyline, props.options, props.trip]);

  if (props.trip !== undefined) {
    return (
      <>
        <Polyline
          positions={props.trip.positions.map((x) => [
            x.coordinates.latitude,
            x.coordinates.longitude
          ])}
          color="red"
        />
        <MapPin
          pin={{
            coordinates: props.trip.startPosition.coordinates,
            color: "green"
          }}
        />
        <MapPin
          pin={{
            coordinates: props.trip.endPosition.coordinates,
            color: "red"
          }}
        />
      </>
    );
  }

  return null;
}
