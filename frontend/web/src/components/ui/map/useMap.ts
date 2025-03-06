import { LatLong } from "@navtrack/shared/api/model/generated";
import L from "leaflet";
import { LatLngExpression } from "leaflet";
import { useCallback } from "react";
import { useMap as useLeafletMap } from "react-leaflet";
import { DEFAULT_MAP_ZOOM_FOR_LIVE_TRACKING } from "../../../constants";
import { MapPadding } from "@navtrack/shared/maps";

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
    (coordinates: LatLong[], padding?: MapPadding) => {
      if (coordinates.length > 0) {
        let featureGroup: L.FeatureGroup<any>;

        if (coordinates.length === 1) {
          featureGroup = L.featureGroup([
            L.marker([coordinates[0].latitude, coordinates[0].longitude]),
            L.marker([
              coordinates[0].latitude - 0.00008999,
              coordinates[0].longitude - 0.00008999
            ]),
            L.marker([
              coordinates[0].latitude + 0.00008999,
              coordinates[0].longitude + 0.00008999
            ])
          ]);
        } else {
          featureGroup = L.featureGroup(
            coordinates.map((x) => L.marker([x.latitude, x.longitude]))
          );
        }

        const bounds = featureGroup.getBounds();

        leafletMap.fitBounds(
          bounds,
          padding
            ? {
                paddingTopLeft: [padding.left, padding.top],
                paddingBottomRight: [padding.right, padding.bottom],
                maxZoom: DEFAULT_MAP_ZOOM_FOR_LIVE_TRACKING
              }
            : undefined
        );
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
