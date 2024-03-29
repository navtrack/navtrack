import { useCallback, useEffect } from "react";
import { useMap } from "react-leaflet";
import { LongLat } from "./mapTypes";

type MapMoveProps = {
  onMove?: (center: LongLat, zoom: number) => void;
};

export function MapMove(props: MapMoveProps) {
  const map = useMap();

  const onMove = useCallback(() => {
    if (props.onMove) {
      const center = map.getCenter();

      props.onMove(
        { latitude: center.lat, longitude: center.lng },
        map.getZoom()
      );
    }
  }, [map, props]);

  useEffect(() => {
    map.on("move", onMove);

    return () => {
      map.off("move", onMove);
    };
  }, [map, onMove]);

  return null;
}
