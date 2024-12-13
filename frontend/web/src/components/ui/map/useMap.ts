import { LatLongModel } from "@navtrack/shared/api/model/generated";
import L from "leaflet";
import { LatLngExpression } from "leaflet";
import { useCallback } from "react";
import { useMap as useLeafletMap } from "react-leaflet";
import { DEFAULT_MAP_ZOOM_FOR_LIVE_TRACKING } from "../../../constants";

export function useMap() {
  const leafletMap = useLeafletMap();

  const setCenter = useCallback(
    (location: LatLngExpression, zoom?: number) => {
      leafletMap.setView(location, zoom, { animate: false });
    },
    [leafletMap]
  );

  const setZoom = useCallback(
    (zoom: number) => {
      leafletMap.setZoom(zoom);
    },
    [leafletMap]
  );

  const fitBounds = useCallback(
    (coordinates: LatLongModel[], options?: L.FitBoundsOptions) => {
      if (coordinates.length > 0) {
        const featureGroup = L.featureGroup(
          coordinates.map((x) => L.marker([x.latitude, x.longitude]))
        );

        const bounds = featureGroup.getBounds();

        if (coordinates.length === 1) {
          leafletMap.setView(
            bounds.getCenter(),
            DEFAULT_MAP_ZOOM_FOR_LIVE_TRACKING
          );
        } else {
          leafletMap.fitBounds(bounds, options);
        }
      }
    },
    [leafletMap]
  );

  return {
    leafletMap,
    fitBounds,
    setCenter,
    setZoom
  };
}
