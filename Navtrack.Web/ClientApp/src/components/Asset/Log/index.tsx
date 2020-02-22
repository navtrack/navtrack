import React, { useState, useEffect } from "react";
import "leaflet/dist/leaflet.css";
import AssetLayout from "components/Framework/Layouts/Asset/AssetLayout";
import { LocationModel } from "services/Api/Model/LocationModel";
import { LocationApi } from "services/Api/LocationApi";
import LocationFilter from "../../Common/LocationFilter/LocationFilter";
import {
  DefaultLocationFilterModel,
  LocationFilterModel
} from "components/Common/LocationFilter/Models/LocationFilterModel";
import MapC from "components/Map";
import useAssetId from "../useAssetId";
import { MapToLocationHistoryRequestModel } from "components/Common/LocationFilter/LocationFilterUtil";
import LocationsTable from "./LocationsTable";

export default function AssetLog() {
  const [locationFilter, setLocationFilter] = useState(DefaultLocationFilterModel);
  const [locations, setLocations] = useState<LocationModel[]>([]);
  const [selectedLocationIndex, setSelectedLocationIndex] = useState(0);
  const [loading, setLoading] = useState(true);
  const assetId = useAssetId();

  const updateLocations = (assetId: number, locationFilter: LocationFilterModel) => {
    setLoading(true);
    let filter = MapToLocationHistoryRequestModel(locationFilter, assetId);
    LocationApi.getHistory(filter)
      .then(x => {
        setLocations(x);
        setSelectedLocationIndex(0);
      })
      .finally(() => setLoading(false));
  };

  useEffect(() => {
    if (assetId) {
      updateLocations(assetId, locationFilter);
    }
  }, [assetId, locationFilter]);

  return (
    <AssetLayout>
      <LocationFilter filter={locationFilter} setFilter={setLocationFilter} />
      <LocationsTable
        loading={loading}
        locations={locations}
        selectedLocationIndex={selectedLocationIndex}
        setSelectedLocationIndex={setSelectedLocationIndex}
      />
      {locations.length > 0 && (
        <div className="bg-white flex-grow rounded shadow" style={{ minHeight: "185px" }}>
          <MapC location={locations[selectedLocationIndex]} />
        </div>
      )}
    </AssetLayout>
  );
}
