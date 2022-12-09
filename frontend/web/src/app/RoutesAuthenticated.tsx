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
import AssetSettingsDevicePage from "../components/asset/settings/device/AssetSettingsDevicePage";
import { useSetCurrentAssetFromRoute } from "@navtrack/ui-shared/hooks/assets/useSetCurrentAssetFromRoute";
import { useSignalR } from "@navtrack/ui-shared/hooks/signalr/useSignalR";

export default function RoutesAuthenticated() {
  useSignalR();
  useSetCurrentAssetFromRoute();

  return (
    <AdminLayout>
      <Switch>
        <Route path={Paths.AssetsLive}>
          <AssetLiveTrackingPage />
        </Route>
        <Route path={Paths.AssetsLog}>
          <AssetLogPage />
        </Route>
        <Route path={Paths.AssetsTrips}>
          <AssetTripsPage />
        </Route>
        <Route path={Paths.AssetsReports}>
          <AssetReportsPage />
        </Route>
        <Route path={Paths.AssetsAlerts}>
          <AssetAlertsPage />
        </Route>
        <Route path={Paths.AssetsSettingsAccess}>
          <AssetSettingsAccessPage />
        </Route>
        <Route path={Paths.AssetsSettingsDevice}>
          <AssetSettingsDevicePage />
        </Route>
        <Route path={Paths.AssetsSettings}>
          <AssetSettingsGeneral />
        </Route>
        <Route path={Paths.AssetsAdd}>
          <AssetAddPage />
        </Route>
        <Route path={Paths.SettingsAccount}>
          <SettingsAccountPage />
        </Route>
        <Route path={Paths.SettingsPassword}>
          <SettingsPasswordPage />
        </Route>
        <Route path={Paths.Home} exact>
          <HomePage />
        </Route>
        <Redirect to={Paths.Home} />
      </Switch>
    </AdminLayout>
  );
}
