import React, { useState, useEffect } from "react";
import {
  DefaultLocationFilterModel,
  LocationFilterModel
} from "components/common/locationFilter/types/LocationFilterModel";
import { LocationModel } from "apis/types/location/LocationModel";
import useAssetId from "framework/hooks/useAssetId";
import { MapToLocationHistoryRequestModel } from "components/common/locationFilter/LocationFilterUtil";
import { LocationApi } from "apis/LocationApi";
import AssetLayout from "components/framework/layouts/asset/AssetLayout";
import LocationFilter from "components/common/locationFilter/LocationFilter";
import LocationsTable from "./LocationsTable";
import MapC from "components/library/map/Map";

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
      .then((x) => {
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
