import { useEffect } from "react";
import { useMap } from "./useMap";

interface IMapCenter {
  latitude?: number;
  longitude?: number;
  zoom?: number;
}

export function MapCenter(props: IMapCenter) {
  const map = useMap();

  useEffect(() => {
    if (props.latitude && props.longitude) {
      const zoom = props.zoom !== undefined ? props.zoom : map.map.getZoom();
      map.setCenter([props.latitude, props.longitude], zoom);
    }
  }, [map, props.latitude, props.longitude, props.zoom]);

  return null;
}
