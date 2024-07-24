import L from "leaflet";
import { Marker } from "react-leaflet";
import { PinIcon } from "./PinIcon";
import { renderToString } from "react-dom/server";
import { useMemo } from "react";
import { MapCenter } from "./MapCenter";
import { LatLongModel } from "@navtrack/shared/api/model/generated";

type MapPinProps = {
  coordinates?: LatLongModel;
  follow?: boolean;
  color?: "primary" | "green" | "red";
  zIndexOffset?: number;
};

export function MapPin(props: MapPinProps) {
  const pin = useMemo(
    () =>
      L.divIcon({
        className: undefined,
        html: renderToString(<PinIcon color={props.color} />),
        iconSize: [28, 42],
        iconAnchor: [14, 35]
      }),
    [props.color]
  );

  if (props.coordinates !== undefined) {
    return (
      <>
        <Marker
          position={[props.coordinates.latitude, props.coordinates.longitude]}
          icon={pin}
          zIndexOffset={props.zIndexOffset}
        />
        {props.follow && <MapCenter position={props.coordinates} />}
      </>
    );
  }

  return null;
}
