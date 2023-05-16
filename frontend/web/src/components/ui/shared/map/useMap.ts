import { LatLngExpression } from "leaflet";
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

  return { map, setCenter, setZoom };
}
