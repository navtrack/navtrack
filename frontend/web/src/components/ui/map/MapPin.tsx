import L from "leaflet";
import { Marker } from "react-leaflet";
import { PinIcon } from "./PinIcon";
import { renderToString } from "react-dom/server";
import { useMemo } from "react";
import { LongLat } from "./types";
import { MapCenter } from "./MapCenter";

type MapPinProps = {
  position?: LongLat;
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

  if (props.position !== undefined) {
    return (
      <>
        <Marker
          position={[props.position.latitude, props.position.longitude]}
          icon={pin}
          zIndexOffset={props.zIndexOffset}
        />
        {props.follow && <MapCenter location={props.position} />}
      </>
    );
  }

  return null;
}
