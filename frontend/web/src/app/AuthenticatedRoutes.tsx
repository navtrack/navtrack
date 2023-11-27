import { Navigate, Route, Routes } from "react-router-dom";
import { HomePage } from "../components/home/HomePage";
import { AssetLiveTrackingPage } from "../components/asset/live-tracking/AssetLiveTrackingPage";
import { AssetLogPage } from "../components/asset/log/AssetLogPage";
import { AssetTripsPage } from "../components/asset/trips/AssetTripsPage";
import { AssetAlertsPage } from "../components/asset/alerts/AssetAlertsPage";
import { AssetReportsPage } from "../components/asset/reports/AssetReportsPage";
import { SettingsAccountPage } from "../components/settings/SettingsAccountPage";
import { AssetSettingsGeneralPage } from "../components/asset/settings/general/AssetSettingsGeneralPage";
import { AssetAddPage } from "../components/asset/add/AssetAddPage";
import { AssetSettingsAccessPage } from "../components/asset/settings/access/AssetSettingsAccessPage";
import { Paths } from "./Paths";
import { SettingsPasswordPage } from "../components/settings/SettingsPasswordPage";
import { AssetSettingsDevicePage } from "../components/asset/settings/device/AssetSettingsDevicePage";
import { ReactNode } from "react";
import { useSetCurrentAssetFromRoute } from "../hooks/assets/useSetCurrentAssetFromRoute";

type AuthenticatedRoutesProps = {
  children?: ReactNode;
};

export function AuthenticatedRoutes(props: AuthenticatedRoutesProps) {
  useSetCurrentAssetFromRoute();

  return (
    <Routes>
      <Route path={Paths.AssetsLive} element={<AssetLiveTrackingPage />} />
      <Route path={Paths.AssetsLog} element={<AssetLogPage />} />
      <Route path={Paths.AssetsTrips} element={<AssetTripsPage />} />
      <Route path={Paths.AssetsReports} element={<AssetReportsPage />} />
      <Route path={Paths.AssetsAlerts} element={<AssetAlertsPage />} />
      <Route path={Paths.AssetsAdd} element={<AssetAddPage />} />
      <Route
        path={Paths.AssetsSettings}
        element={<AssetSettingsGeneralPage />}
      />
      <Route
        path={Paths.AssetsSettingsDevice}
        element={<AssetSettingsDevicePage />}
      />
      <Route
        path={Paths.AssetsSettingsAccess}
        element={<AssetSettingsAccessPage />}
      />
      <Route path={Paths.SettingsPassword} element={<SettingsPasswordPage />} />
      <Route path={Paths.SettingsAccount} element={<SettingsAccountPage />} />
      {props.children}
      <Route path={Paths.Home} element={<HomePage />} />
      <Route path="*" element={<Navigate replace to={Paths.Home} />} />
    </Routes>
  );
}
