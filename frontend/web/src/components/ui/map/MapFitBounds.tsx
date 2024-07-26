import { useEffect } from "react";
import { useMap } from "./useMap";
import { LatLongModel } from "@navtrack/shared/api/model/generated";

type MapFitBoundsProps = {
  coordinates?: LatLongModel[];
};

export function MapFitBounds(props: MapFitBoundsProps) {
  const map = useMap();

  useEffect(() => {
    if (props.coordinates !== undefined && props.coordinates?.length > 0) {
      map.showAll(props.coordinates);
    }
  }, [map, props.coordinates]);

  return null;
}
