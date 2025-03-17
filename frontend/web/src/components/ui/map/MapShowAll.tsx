import { useEffect, useState } from "react";
import { useMap } from "./useMap";
import { LatLong } from "@navtrack/shared/api/model";

type MapFitBoundsProps = {
  coordinates?: LatLong[];
  once?: boolean;
};

export function MapShowAll2(props: MapFitBoundsProps) {
  const map = useMap();
  const [initialized, setInitialized] = useState(false);

  useEffect(() => {
    if (
      props.coordinates !== undefined &&
      props.coordinates.length > 0 &&
      ((props.once && !initialized) || !props.once)
    ) {
      map.fitBounds(props.coordinates);

      setInitialized(true);
    }
  }, [initialized, map, props.coordinates, props.once]);

  return null;
}
