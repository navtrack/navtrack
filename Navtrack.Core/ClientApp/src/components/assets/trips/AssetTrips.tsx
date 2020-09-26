import { LatLng } from "leaflet";
import React, { useState, useEffect } from "react";
import { AssetApi } from "../../../apis/AssetApi";
import { DefaultGetTripsModel } from "../../../apis/types/asset/GetTripsModel";
import useAssetId from "../../../services/hooks/useAssetId";
import useMap from "../../../services/hooks/useMap";
import { MapService } from "../../../services/MapService";
import LocationFilter from "../../common/locationFilter/LocationFilter";
import { MapToLocationHistoryRequestModel } from "../../common/locationFilter/LocationFilterUtil";
import {
  DefaultLocationFilterModel,
  LocationFilterModel
} from "../../common/locationFilter/types/LocationFilterModel";
import AssetTripsTable from "./AssetTripsTable";

export default function AssetTrips() {
  useMap();
  const [locationFilter, setLocationFilter] = useState(DefaultLocationFilterModel);
  const [trips, setLocations] = useState(DefaultGetTripsModel);
  const [selectedIndex, setSelectedLocationIndex] = useState(0);
  const [loading, setLoading] = useState(true);
  const assetId = useAssetId();

  const updateLocations = (assetId: number, locationFilter: LocationFilterModel) => {
    setLoading(true);
    let filter = MapToLocationHistoryRequestModel(locationFilter, assetId);
    AssetApi.getTrips(assetId, filter)
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

  useEffect(() => {
    if (
      trips &&
      trips.results &&
      trips.results[selectedIndex] &&
      trips.results[selectedIndex].locations
    ) {
      MapService.displayRoute(
        trips.results[selectedIndex].locations.map((x) => new LatLng(x.latitude, x.longitude))
      );
    }
  }, [selectedIndex, trips]);

  return (
    <>
      <LocationFilter filter={locationFilter} setFilter={setLocationFilter} />
      <AssetTripsTable
        loading={loading}
        model={trips}
        selectedIndex={selectedIndex}
        setSelectedIndex={setSelectedLocationIndex}
      />
    </>
  );
}
