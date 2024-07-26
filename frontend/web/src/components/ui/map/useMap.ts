import { LatLongModel } from "@navtrack/shared/api/model/generated";
import { LatLngExpression, LatLngTuple } from "leaflet";
import { useCallback } from "react";
import { useMap as useLeafletMap } from "react-leaflet";

export function useMap() {
  const map = useLeafletMap();

  const setCenter = useCallback(
    (location: LatLngExpression, zoom?: number) => {
      map.setView(location, zoom, { animate: false });
    },
    [map]
  );

  const setZoom = useCallback(
    (zoom: number) => {
      map.setZoom(zoom);
    },
    [map]
  );

  const showAll = useCallback(
    (coordinates: LatLongModel[], customPadding: number = 100) => {
      const latLngs = coordinates.map((position) => {
        return [position.latitude, position.longitude] as LatLngTuple;
      });

      map.fitBounds(latLngs, { padding: [customPadding, customPadding] });
    },
    [map]
  );

  return { leafletMap: map, setCenter, setZoom, showAll };
}
