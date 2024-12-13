import { useEffect } from "react";
import { useMap } from "./useMap";

type MapResizeObserverProps = {
  mapContainerRef: React.RefObject<HTMLDivElement>;
};

// This component is used to observe the resize of the map container and invalidate the map size.
export function MapResizeObserver(props: MapResizeObserverProps) {
  const map = useMap();

  useEffect(() => {
    if (!props.mapContainerRef.current) {
      return;
    }

    const resizeObserver = new ResizeObserver(() => {
      map.leafletMap.invalidateSize();
    });

    resizeObserver.observe(props.mapContainerRef.current);

    return () => resizeObserver.disconnect();
  }, [map.leafletMap, props.mapContainerRef]);

  return null;
}
