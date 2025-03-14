import { LatLong } from "@navtrack/shared/api/model/generated";
import L from "leaflet";
import { LatLngExpression } from "leaflet";
import { useCallback } from "react";
import { useMap as useLeafletMap } from "react-leaflet";
import { MapOptionsDto } from "@navtrack/shared/maps";

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
    (coordinates: LatLong[], options?: MapOptionsDto) => {
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
        const zoom = leafletMap.getZoom();

        leafletMap.fitBounds(
          bounds,
          options?.padding
            ? {
                paddingTopLeft: [options?.padding.left, options?.padding.top],
                paddingBottomRight: [
                  options?.padding.right,
                  options?.padding.bottom
                ],
                maxZoom: options.initialZoom ?? zoom
              }
            : undefined
        );
      }
    },
    [leafletMap]
  );

  return { leafletMap, fitBounds, setCenter, setZoom };
}
