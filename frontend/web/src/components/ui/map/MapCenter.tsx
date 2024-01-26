import { useEffect } from "react";
import { useMap } from "./useMap";
import { LongLat } from "./types";

type MapCenterProps = {
  location?: LongLat;
  zoom?: number;
};

export function MapCenter(props: MapCenterProps) {
  const map = useMap();

  useEffect(() => {
    if (props.location) {
      const currentCenter = map.leafletMap.getCenter();

      if (
        currentCenter.lat !== props.location.latitude ||
        currentCenter.lng !== props.location.longitude
      ) {
        const zoom = props.zoom !== undefined ? props.zoom : map.leafletMap.getZoom();

        map.setCenter(
          [props.location.latitude, props.location.longitude],
          zoom
        );
      }
    }
  }, [map, props.location, props.zoom]);

  return null;
}
