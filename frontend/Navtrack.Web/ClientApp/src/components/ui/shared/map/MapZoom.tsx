import { useEffect } from "react";
import useMap from "./useMap";

interface IMapZoom {
  zoom?: number;
}

export function MapZoom(props: IMapZoom) {
  const map = useMap();

  useEffect(() => {
    if (props.zoom !== undefined) {
      map.map.setZoom(props.zoom);
    }
  }, [map, props.zoom]);

  return null;
}
