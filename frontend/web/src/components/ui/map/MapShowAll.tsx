import { useEffect, useState } from "react";
import { useMap } from "./useMap";
import { LatLongModel } from "@navtrack/shared/api/model/generated";

type MapFitBoundsProps = {
  coordinates?: LatLongModel[];
  once?: boolean;
};

export function MapShowAll(props: MapFitBoundsProps) {
  const map = useMap();
  const [initalized, setInitialized] = useState(false);

  useEffect(() => {
    if (
      (props.coordinates?.length ?? 0) > 0 &&
      ((props.once && !initalized) || !props.once)
    ) {
      map.showAllMarkers(props.coordinates);
      setInitialized(true);
    }
  }, [initalized, map, props.coordinates, props.once]);

  return null;
}
