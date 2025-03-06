import L from "leaflet";
import { Marker } from "react-leaflet";
import { PinIcon } from "./PinIcon";
import { renderToString } from "react-dom/server";
import { useEffect, useMemo } from "react";
import { MapPadding, FeMapPin } from "@navtrack/shared/maps";
import { useMap } from "./useMap";

type MapPinProps = {
  pin: FeMapPin;
  zIndexOffset?: number;
  padding?: MapPadding;
};

export function MapPin(props: MapPinProps) {
  const map = useMap();

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

  useEffect(() => {
    if (props.pin.follow && props.pin.coordinates) {
      map.fitBounds([props.pin.coordinates], props.padding);
    }
  }, [map, props.padding, props.pin.coordinates, props.pin.follow]);

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
        {/* {props.pin.follow && <MapCenter position={props.pin.coordinates} />} */}
      </>
    );
  }

  return null;
}
