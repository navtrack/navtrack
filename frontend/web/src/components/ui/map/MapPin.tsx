import L from "leaflet";
import { Marker } from "react-leaflet";
import { PinIcon } from "./PinIcon";
import { renderToString } from "react-dom/server";
import { useEffect, useMemo, useState } from "react";
import { FeMapPin, MapOptions } from "@navtrack/shared/maps";
import { useMap } from "./useMap";

type MapPinProps = {
  pin: FeMapPin;
  zIndexOffset?: number;
  mapOptions?: MapOptions;
};

export function MapPin(props: MapPinProps) {
  const map = useMap();
  const [initialZoomSet, setInitialZoomSet] = useState(false);

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
      const initialZoom =
        props.mapOptions?.initialZoom && !initialZoomSet
          ? props.mapOptions.initialZoom
          : undefined;

      if (initialZoom) {
        setInitialZoomSet(true);
      }

      map.fitBounds([props.pin.coordinates], {
        ...props.mapOptions,
        initialZoom: initialZoom
      });
    }
  }, [
    initialZoomSet,
    map,
    props.mapOptions,
    props.pin.coordinates,
    props.pin.follow
  ]);

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
      </>
    );
  }

  return null;
}
