import React, { useState, useEffect, useContext } from "react";
import "leaflet/dist/leaflet.css"
import { LocationInfo } from "./LocationInfo";
import { useParams } from "react-router";
import AppContext from "services/AppContext";
import { AssetModel, DefaultAssetModel } from "services/Api/Model/AssetModel";
import AssetLayout from "components/Framework/Layouts/Asset/AssetLayout";
import MapC from "components/Map";

export default function LiveTracking() {
  const { appContext } = useContext(AppContext);
  let { assetId } = useParams();
  const [asset, setAsset] = useState<AssetModel>(DefaultAssetModel);

  useEffect(() => {
    if (assetId !== undefined) {
      const id = parseInt(assetId);
      const filteredAsset = appContext.assets?.find(x => x.id === id);

      if (filteredAsset) {
        setAsset(filteredAsset);
      }
    }
  }, [appContext.assets, assetId]);

  return (
    <>
      {asset &&
        <AssetLayout id={asset.id} name={asset.name}>
          <div className="bg-white shadow p-3 rounded mb-3 flex">
            {asset.location ?
              <>
                <LocationInfo title="Location" hideMargin={true}>{asset.location.dateTime?.toString()}</LocationInfo>
                <LocationInfo title="Date" hideMargin={true}>{asset.location.dateTime?.toString()}</LocationInfo>
                <LocationInfo title="Latitude">{asset.location.latitude}</LocationInfo>
                <LocationInfo title="Longitude">{asset.location.longitude}</LocationInfo>
                <LocationInfo title="Altitude">{asset.location.altitude} m</LocationInfo>
                <LocationInfo title="Speed">{asset.location.speed} km/h</LocationInfo>
                <LocationInfo title="Heading">{asset.location.heading}°</LocationInfo>
                <LocationInfo title="Satellites">{asset.location.satellites}</LocationInfo>
                <LocationInfo title="HDOP">{asset.location.hdop}</LocationInfo>
              </>
              :
              <>No location data available.</>
            }
          </div>
          {asset.location &&
            <div className="flex-grow shadow rounded">
              <MapC location={asset.location} />
            </div>
          }
        </AssetLayout>
      }
    </>
  );
}