import { LatLong } from "@navtrack/shared/api/model";
import { useCallback, useEffect } from "react";
import { useMap } from "react-leaflet";

type MapMoveProps = {
  onMove?: (center: LatLong, zoom: number) => void;
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
