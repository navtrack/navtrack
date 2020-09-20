import React, { useEffect } from "react";
import useAsset from "../../../services/hooks/useAsset";
import { MapService } from "../../../services/MapService";
import { LocationInfo } from "./LocationInfo";
import useMap from "../../../services/hooks/useMap";
import { showDate } from "../../../services/util/DateUtil";

export default function AssetLive() {
  useMap();
  const asset = useAsset();

  useEffect(() => {
    if (asset) {
      MapService.showMarker([asset.location.latitude, asset.location.longitude]);
    }
  }, [asset]);

  return (
    <>
      {asset && (
        <div
          className="p-3 flex flex-col"
          style={{
            minWidth: "630px"
          }}>
          <div className="flex">
            {asset.location ? (
              <>
                <LocationInfo title="Date" hideMargin={true}>
                  {showDate(asset.location.dateTime)}
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
        </div>
      )}
    </>
  );
}
