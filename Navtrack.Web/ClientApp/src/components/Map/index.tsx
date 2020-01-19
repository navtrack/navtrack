import React, { useEffect, useState } from "react";
import { Map, TileLayer, Popup, Marker, useLeaflet } from "react-leaflet"

import "leaflet/dist/leaflet.css"
import { LocationModel } from "../../services/Api/Model/LocationModel";
import { LatLngExpression } from "leaflet";

type Props = {
    location: LocationModel
}

export default function MapC(props: Props) {
    const [zoom] = useState(13);
    const [position, setPosition] = useState<LatLngExpression>([props.location.latitude, props.location.longitude]);

    useEffect(() => {
        setPosition([props.location.latitude, props.location.longitude]);
    }, [props.location]);

    return (
        <Map center={position} zoom={zoom} className="rounded flex-fill">
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
    location: LocationModel
}

function Location(props: LocationProps) {
    const { map } = useLeaflet();

    useEffect(() => {
        if (map) {
            map.panTo([props.location.latitude, props.location.longitude]);
        }
    }, [props.location, map])

    return (<></>);
}