import { useEffect, useState } from "react";
import { useMap } from "./useMap";
import { LatLng } from "./types";

type MapCenterProps = {
  location?: LatLng;
  zoom?: number;
};

export function MapCenter(props: MapCenterProps) {
  const map = useMap();
  const [currentLocation, setCurrentLocation] = useState<LatLng>();

  useEffect(() => {
    if (props.location && props.location !== currentLocation) {
      const zoom = props.zoom !== undefined ? props.zoom : map.map.getZoom();

      map.setCenter([props.location.latitude, props.location.longitude], zoom);
      setCurrentLocation(props.location);
    }
  }, [currentLocation, map, props.location, props.zoom]);

  return null;
}
