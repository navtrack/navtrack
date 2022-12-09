import L, { LatLng, Polyline } from "leaflet";
import { useEffect, useState } from "react";
import useMap from "./useMap";
import MapPin from "./MapPin";
import { TripModel } from "@navtrack/ui-shared/api/model/generated";

interface MapTripProps {
  trip?: TripModel;
}

export const MapTrip = (props: MapTripProps) => {
  const map = useMap();
  const [polyline, setPolyline] = useState<Polyline | undefined>(undefined);
  const [polylineVisible, setPolylineVisible] = useState(false);

  useEffect(() => {
    if (props.trip && !polylineVisible) {
      const latlngs = props.trip.locations.map(
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
        <MapPin
          latitude={props.trip.startLocation.latitude}
          longitude={props.trip.startLocation.longitude}
          color="green"
        />
        <MapPin
          latitude={props.trip.endLocation.latitude}
          longitude={props.trip.endLocation.longitude}
          color="red"
        />
      </>
    );
  }

  return null;
};
