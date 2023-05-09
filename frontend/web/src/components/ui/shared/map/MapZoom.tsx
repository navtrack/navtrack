import { useEffect } from "react";
import { useMap } from "./useMap";

type MapZoomProps = {
  zoom?: number;
};

export function MapZoom(props: MapZoomProps) {
  const map = useMap();

  useEffect(() => {
    if (props.zoom !== undefined && map.map !== undefined) {
      map.map.setZoom(props.zoom);
    }
  }, [map, props.zoom]);

  return null;
}
