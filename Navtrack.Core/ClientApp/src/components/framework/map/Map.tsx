import React, { useEffect, useState } from "react";
import { Map, TileLayer, Popup, Marker, useLeaflet } from "react-leaflet";
import "leaflet/dist/leaflet.css";
import { LatLngExpression } from "leaflet";
import { LocationModel } from "services/api/types/location/LocationModel";

type Props = {
  location: LocationModel;
};

export default function MapC(props: Props) {
  const [zoom] = useState(13);
  const [position, setPosition] = useState<LatLngExpression>([
    props.location.latitude,
    props.location.longitude
  ]);

  useEffect(() => {
    setPosition([props.location.latitude, props.location.longitude]);
  }, [props.location]);

  React.useEffect(() => {
    const L = require("leaflet");

    delete L.Icon.Default.prototype._getIconUrl;

    L.Icon.Default.mergeOptions({
      iconRetinaUrl: require("leaflet/dist/images/marker-icon-2x.png"),
      iconUrl: require("leaflet/dist/images/marker-icon.png"),
      shadowUrl: require("leaflet/dist/images/marker-shadow.png")
    });
  }, []);

  return (
    <Map center={position} zoom={zoom} className="flex w-full h-full rounded z-0">
      <Location location={props.location} />
      <TileLayer url="https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png" />
      <Marker position={position}>
        <Popup>
          A pretty CSS3 popup. <br /> Easily customizable.
        </Popup>
      </Marker>
    </Map>
  );
}

type LocationProps = {
  location: LocationModel;
};

function Location(props: LocationProps) {
  const { map } = useLeaflet();

  useEffect(() => {
    if (map) {
      map.panTo([props.location.latitude, props.location.longitude]);
    }
  }, [props.location, map]);

  return <></>;
}
