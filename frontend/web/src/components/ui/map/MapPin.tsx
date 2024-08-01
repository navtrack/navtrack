import L from "leaflet";
import { Marker } from "react-leaflet";
import { PinIcon } from "./PinIcon";
import { renderToString } from "react-dom/server";
import { useMemo } from "react";
import { MapCenter } from "./MapCenter";
import { MapPinUiModel } from "@navtrack/shared/models/maps";

type MapPinProps = {
  pin: MapPinUiModel;
  zIndexOffset?: number;
};

export function MapPin(props: MapPinProps) {
  const pin = useMemo(
    () =>
      L.divIcon({
        className: undefined,
        html: renderToString(<PinIcon color={props.pin.color} />),
        iconSize: [28, 42],
        iconAnchor: [14, 35]
      }),
    [props.pin.color]
  );

  if (props.pin.coordinates !== undefined) {
    return (
      <>
        <Marker
          position={[
            props.pin.coordinates.latitude,
            props.pin.coordinates.longitude
          ]}
          icon={pin}
          zIndexOffset={props.zIndexOffset}
        />
        {props.pin.follow && <MapCenter position={props.pin.coordinates} />}
      </>
    );
  }

  return null;
}
