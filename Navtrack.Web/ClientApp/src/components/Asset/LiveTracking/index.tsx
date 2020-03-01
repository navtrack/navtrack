import React from "react";
import "leaflet/dist/leaflet.css";
import { LocationInfo } from "./LocationInfo";
import MapC from "components/Map";
import AssetLayout from "components/Framework/Layouts/Asset/AssetLayout";
import useAsset from "../useAsset";

export default function LiveTracking() {
  const asset = useAsset();

  return (
    <AssetLayout>
      {asset && (
        <>
          <div className="bg-white shadow p-3 rounded mb-3 flex">
            {asset.location ? (
              <>
                <LocationInfo title="Date" hideMargin={true}>
                  {asset.location.dateTime}
                </LocationInfo>
                <LocationInfo title="Latitude">{asset.location.latitude}</LocationInfo>
                <LocationInfo title="Longitude">{asset.location.longitude}</LocationInfo>
                <LocationInfo title="Altitude">{asset.location.altitude} m</LocationInfo>
                <LocationInfo title="Speed">{asset.location.speed} km/h</LocationInfo>
                <LocationInfo title="Heading">{asset.location.heading}°</LocationInfo>
                <LocationInfo title="Satellites">{asset.location.satellites}</LocationInfo>
                <LocationInfo title="HDOP">{asset.location.hdop}</LocationInfo>
              </>
            ) : (
              <>No location data available.</>
            )}
          </div>
          {asset.location && (
            <div className="flex-grow shadow rounded">
              <MapC location={asset.location} />
            </div>
          )}
        </>
      )}
    </AssetLayout>
  );
}
