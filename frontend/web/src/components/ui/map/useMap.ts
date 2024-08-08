import { LatLongModel } from "@navtrack/shared/api/model/generated";
import L from "leaflet";
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

  const showAllMarkers = useCallback(
    (
      coordinates: LatLongModel[] | undefined = undefined,
      customPadding: number = 100
    ) => {
      let latLngs: LatLngTuple[] = [];

      if (coordinates !== undefined) {
        latLngs = coordinates.map((position) => {
          return [position.latitude, position.longitude] as LatLngTuple;
        });
      } else {
        map.eachLayer((layer) => {
          if (layer instanceof L.Marker) {
            const latLng = (layer as L.Marker).getLatLng();

            latLngs.push([latLng.lat, latLng.lng]);
          }
        });
      }

      if (latLngs.length > 0) {
        map.fitBounds(latLngs);
        // TODO
        // map.fitBounds(latLngs, { padding: [customPadding, customPadding] });
      }
    },
    [map]
  );

  return {
    leafletMap: map,
    setCenter,
    setZoom,
    showAllMarkers
  };
}
