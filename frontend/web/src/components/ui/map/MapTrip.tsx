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
  const [polyline, setPolyline] = useState<Polyline | undefined>(undefined);
  const [polylineVisible, setPolylineVisible] = useState(false);

  useEffect(() => {
    if (props.trip && !polylineVisible) {
      const latlngs = props.trip.positions.map(
        (x) => new LatLng(x.latitude, x.longitude)
      );

      const polyline = L.polyline(latlngs, { color: "red" }).addTo(map.map);

      map.map.fitBounds(polyline.getBounds(), { padding: [30, 30] });

      setPolyline(polyline);
      setPolylineVisible(true);
    }

    return () => {
      if (polyline && polylineVisible) {
        polyline.removeFrom(map.map);
        setPolylineVisible(false);
      }
    };
  }, [map, polyline, polylineVisible, props.trip]);

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
