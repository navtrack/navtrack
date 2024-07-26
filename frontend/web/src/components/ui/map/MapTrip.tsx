import L, { LatLng, Polyline } from "leaflet";
import { useEffect, useState } from "react";
import { useMap } from "./useMap";
import { MapPin } from "./MapPin";
import { TripModel } from "@navtrack/shared/api/model/generated";
import { MapPinLabel } from "./MapPinLabel";

type MapTripProps = {
  trip?: TripModel;
};

export function MapTrip(props: MapTripProps) {
  const map = useMap();
  const [trip, setTrip] = useState(props.trip);
  const [polyline, setPolyline] = useState<Polyline | undefined>(undefined);

  useEffect(() => {
    if (trip !== props.trip) {
      if (polyline !== undefined) {
        polyline.removeFrom(map.leafletMap);
      }

      if (props.trip !== undefined) {
        const latlngs = props.trip.positions.map(
          (x) => new LatLng(x.coordinates.latitude, x.coordinates.longitude)
        );

        const pl = L.polyline(latlngs, { color: "red" });
        pl.addTo(map.leafletMap);
        setPolyline(pl);
        map.showAll(
          props.trip.positions.map((x) => x.coordinates),
          100
        );
      }

      setTrip(props.trip);
    }
  }, [map, map.leafletMap, polyline, props.trip, trip]);

  if (props.trip) {
    return (
      <>
        <MapPin
          coordinates={props.trip.startPosition.coordinates}
          color="green"
        />
        <MapPin coordinates={props.trip.endPosition.coordinates} color="red" />
        <MapPin coordinates={props.trip.endPosition.coordinates} color="red" />
      </>
    );
  }

  return null;
}
