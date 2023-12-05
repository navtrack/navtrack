import { useEffect, useState } from "react";
import { useMap } from "./useMap";

type MapZoomProps = {
  initialZoom?: number;
};

export function MapZoom(props: MapZoomProps) {
  const map = useMap();
  const [initialized, setInitialized] = useState(false);

  useEffect(() => {
    if (
      !initialized &&
      props.initialZoom !== undefined &&
      map.map !== undefined
    ) {
      map.map.setZoom(props.initialZoom);
      setInitialized(true);
    }
  }, [initialized, map, props.initialZoom]);

  return null;
}
