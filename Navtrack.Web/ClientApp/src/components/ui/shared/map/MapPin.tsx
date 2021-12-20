import L from "leaflet";
import { Marker } from "react-leaflet";
import PinIcon from "./PinIcon";
import { renderToString } from "react-dom/server";
import { useEffect, useMemo } from "react";
import useMap from "./useMap";

interface IMapPin {
  latitude?: number;
  longitude?: number;
  follow?: boolean;
  color?: "primary" | "green" | "red";
  zIndexOffset?: number;
}

export default function MapPin(props: IMapPin) {
  const map = useMap();

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

  useEffect(() => {
    if (props.follow && props.latitude && props.longitude) {
      map.setCenter([props.latitude, props.longitude]);
    }
  }, [map, props.follow, props.latitude, props.longitude]);

  if (props.latitude && props.longitude) {
    return (
      <Marker
        position={[props.latitude, props.longitude]}
        icon={pin}
        zIndexOffset={props.zIndexOffset}></Marker>
    );
  }

  return null;
}
