import L, { LatLng, Polyline } from "leaflet";
import { useEffect, useState } from "react";
import { useMap } from "./useMap";
import { MapPin } from "./MapPin";
import { TripModel } from "@navtrack/shared/api/model/generated";

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
          (x) => new LatLng(x.latitude, x.longitude)
        );

        const pl = L.polyline(latlngs, { color: "red" });
        pl.addTo(map.leafletMap);
        map.leafletMap.fitBounds(pl.getBounds(), { padding: [30, 30] });
        setPolyline(pl);
      }

      setTrip(props.trip);
    }
  }, [map.leafletMap, polyline, props.trip, trip]);

  if (props.trip) {
    return (
      <>
        <MapPin location={{ ...props.trip.startPosition }} color="green" />
        <MapPin location={{ ...props.trip.endPosition }} color="red" />
      </>
    );
  }

  return null;
}
