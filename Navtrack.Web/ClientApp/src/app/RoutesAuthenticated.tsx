import { Redirect, Route, Switch } from "react-router-dom";
import HomePage from "../components/home/HomePage";
import AssetLiveTrackingPage from "../components/asset/live-tracking/AssetLiveTrackingPage";
import AssetLogPage from "../components/asset/log/AssetLogPage";
import AssetTripsPage from "../components/asset/trips/AssetTripsPage";
import AssetAlertsPage from "../components/asset/alerts/AssetAlertsPage";
import AssetReportsPage from "../components/asset/reports/AssetReportsPage";
import SettingsAccountPage from "../components/settings/SettingsAccountPage";
import AdminLayout from "../components/ui/layouts/admin/AdminLayout";
import AssetSettingsGeneral from "../components/asset/settings/general/AssetSettingsGeneralPage";
import AssetAddPage from "../components/asset/add/AssetAddPage";
import AssetSettingsAccessPage from "../components/asset/settings/access/AssetSettingsAccessPage";
import Paths from "./Paths";
import SettingsPasswordPage from "../components/settings/SettingsPasswordPage";
import useSignalR from "../hooks/app/useSignalR";
import AssetSettingsDevicePage from "../components/asset/settings/device/AssetSettingsDevicePage";

export default function RoutesAuthenticated() {
  useSignalR();

  return (
    <AdminLayout>
      <Switch>
        <Route path="/assets/:id/live">
          <AssetLiveTrackingPage />
        </Route>
        <Route path="/assets/:id/log">
          <AssetLogPage />
        </Route>
        <Route path="/assets/:id/trips">
          <AssetTripsPage />
        </Route>
        <Route path="/assets/:id/reports">
          <AssetReportsPage />
        </Route>
        <Route path="/assets/:id/alerts">
          <AssetAlertsPage />
        </Route>
        <Route path={Paths.AssetSettingsAccess}>
          <AssetSettingsAccessPage />
        </Route>
        <Route path={Paths.AssetSettingsDevice}>
          <AssetSettingsDevicePage />
        </Route>
        <Route path={Paths.AssetSettings}>
          <AssetSettingsGeneral />
        </Route>
        <Route path="/assets/add">
          <AssetAddPage />
        </Route>
        <Route path="/settings/account">
          <SettingsAccountPage />
        </Route>
        <Route path="/settings/password">
          <SettingsPasswordPage />
        </Route>
        <Route path="/">
          <HomePage />
        </Route>
        <Redirect from="*" to="/" />
      </Switch>
    </AdminLayout>
  );
}
