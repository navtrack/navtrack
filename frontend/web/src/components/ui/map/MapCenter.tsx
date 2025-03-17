import { useEffect } from "react";
import { useMap } from "./useMap";
import { LatLong } from "@navtrack/shared/api/model";

type MapCenterProps = {
  position?: LatLong;
  zoom?: number;
};

export function MapCenter(props: MapCenterProps) {
  const map = useMap();

  useEffect(() => {
    if (props.position) {
      const currentCenter = map.leafletMap.getCenter();

      if (
        currentCenter.lat !== props.position.latitude ||
        currentCenter.lng !== props.position.longitude
      ) {
        const zoom =
          props.zoom !== undefined ? props.zoom : map.leafletMap.getZoom();

        map.setCenter(
          [props.position.latitude, props.position.longitude],
          zoom
        );
      }
    }
  }, [map, props.position, props.zoom]);

  return null;
}
