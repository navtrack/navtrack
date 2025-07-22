import { LatLong } from "@navtrack/shared/api/model";
import { useGeocodeReverseQuery } from "../../../hooks/queries/geocode/useGeocodeReverseQuery";
import { Fragment } from "react";

type GeocodeReverseProps = {
  coordinates?: LatLong;
};

export function GeocodeReverse(props: GeocodeReverseProps) {
  const location = useGeocodeReverseQuery({
    lat: props.coordinates?.latitude,
    lon: props.coordinates?.longitude
  });

  return <Fragment>{location.data?.displayName ?? ""}</Fragment>;
}
