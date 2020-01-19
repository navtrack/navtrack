import React, { useState, useEffect, ReactNode, useCallback } from "react";
import "leaflet/dist/leaflet.css"
import MapC from "../Map";
import { LocationModel, DefaultLocationModel } from "../../services/Api/Model/LocationModel";
import { LocationApi } from "../../services/Api/LocationApi";
import { useParams } from "react-router";
import AssetLayout from "components/Framework/Layouts/Admin/AssetLayout";

export default function LiveTracking() {
    let { assetId } = useParams();
    const [location, setLocation] = useState<LocationModel>(DefaultLocationModel);

    const updateLocation = useCallback(() => {
        if (assetId) {
            LocationApi.getLatest(+assetId)
                .then((x) => setLocation(x));
        }
    }, [assetId]);

    useEffect(() => {
        updateLocation();
        setInterval(updateLocation, 1000 * 10); // TODO replace with signalr
    }, [assetId, updateLocation]);

    return (
        <AssetLayout id={2} name={"BN01SBU"}>
                <div className="card shadow mb-3">
                    <div className="card-body py-3">
                        <LocationInfo title="Date" hideMargin={true}><>{location.dateTime?.toString()}</></LocationInfo>
                        <LocationInfo title="Latitude">{location.latitude}</LocationInfo>
                        <LocationInfo title="Longitude">{location.longitude}</LocationInfo>
                        <LocationInfo title="Altitude">{location.altitude} m</LocationInfo>
                        <LocationInfo title="Speed">{location.speed} km/h</LocationInfo>
                        <LocationInfo title="Heading">{location.heading}°</LocationInfo>
                        <LocationInfo title="Satellites">{location.satellites}</LocationInfo>
                        <LocationInfo title="HDOP">{location.hdop}</LocationInfo>
                    </div>
                </div>
                <div className="card shadow flex-fill">
                    <MapC location={location} />
                </div>
        </AssetLayout>
    );
}

type LocationInfoProps = {
    title: string,
    children: ReactNode,
    hideMargin?: boolean
}

function LocationInfo(props: LocationInfoProps) {
    return (
        <div className={`float-left mr-4`}>
            <h5 className="mb-0 text-muted">{props.title}</h5>
            <h4 className="mb-0">{props.children}</h4>
        </div>
    );
}