import { useEffect } from "react";
import { useMap } from "./useMap";
import { LatLngTuple } from "leaflet";
import { LatLongModel } from "@navtrack/shared/api/model/generated";

type MapFitBoundsProps = {
  coordinates?: LatLongModel[];
};

export function MapFitBounds(props: MapFitBoundsProps) {
  const map = useMap();

  useEffect(() => {
    if (props.coordinates !== undefined && props.coordinates?.length > 0) {
      const latLngs = props.coordinates.map((position) => {
        return [position.latitude, position.longitude] as LatLngTuple;
      });

      map.leafletMap.fitBounds(latLngs);
    }
  }, [map.leafletMap, props.coordinates]);

  return null;
}
