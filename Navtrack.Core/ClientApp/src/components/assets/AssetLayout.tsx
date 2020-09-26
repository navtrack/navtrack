import React from "react";
import AssetLayoutNavbar from "./AssetLayoutNavbar";
import { Route, Switch } from "react-router";
import AssetLive from "./live/AssetLive";
import AssetLocations from "./locations/AssetLocations";
import AssetSettingsLayout from "./settings/AssetSettingsLayout";
import AssetTrips from "./trips/AssetTrips";

export default function AssetLayout() {
  return (
    <>
      <AssetLayoutNavbar />
      <div className="flex flex-col flex-grow z-20 p-3">
        <div className="shadow bg-white rounded">
          <Switch>
            <Route path={"/assets/:assetId/live"}>
              <AssetLive />
            </Route>
            <Route path={"/assets/:assetId/locations"}>
              <AssetLocations />
            </Route>
            <Route path={"/assets/:assetId/trips"}>
              <AssetTrips />
            </Route>
            <Route path={"/assets/:assetId/settings"}>
              <AssetSettingsLayout />
            </Route>
          </Switch>
        </div>
      </div>
    </>
  );
}
