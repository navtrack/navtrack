import L, { LatLng, Polyline as LeafletPolyline } from "leaflet";
import { useEffect, useState } from "react";
import { useMap } from "./useMap";
import { MapPin } from "./MapPin";
import { Trip } from "@navtrack/shared/api/model/generated";
import { Polyline } from "react-leaflet";
import { MapOptions } from "@navtrack/shared/maps";

type MapTripProps = { trip?: Trip; options?: MapOptions };

export function MapTrip(props: MapTripProps) {
  const map = useMap();
  const [polyline, setPolyline] = useState<LeafletPolyline | undefined>(
    undefined
  );
  const [currentTrip, setCurrentTrip] = useState<Trip | undefined>(undefined);

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
