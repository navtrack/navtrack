import React from "react";
import "leaflet/dist/leaflet.css";
import { LocationInfo } from "./LocationInfo";
import useAsset from "framework/hooks/useAsset";
import AssetLayout from "components/framework/layouts/asset/AssetLayout";
import MapC from "components/library/map/Map";

export default function AssetLive() {
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
