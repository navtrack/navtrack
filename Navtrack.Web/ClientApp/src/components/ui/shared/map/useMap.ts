import { LatLngExpression } from "leaflet";
import { useCallback } from "react";
import { useMap as useLeafletMap } from "react-leaflet";

export default function useMap() {
  const map = useLeafletMap();

  const setCenter = useCallback(
    (location: LatLngExpression, zoom?: number) => {
      map.setView(location, zoom, { animate: false });
    },
    [map]
  );

  return { map, setCenter };
}
