import { Navigate, Route, Routes } from "react-router-dom";
import { HomePage } from "../components/home/HomePage";
import { AssetLiveTrackingPage } from "../components/asset/live-tracking/LiveTrackingPage";
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
import { AuthenticatedLayoutTwoColumns } from "../components/ui/layouts/authenticated/AuthenticatedLayoutTwoColumns";
import { AssetLogPage } from "../components/asset/log/AssetLogPage";
import { AssetSettingsLayout } from "../components/asset/settings/shared/AssetSettingsLayout";
import { AccountSettingsLayout } from "../components/settings/AccountSettingsLayout";

type AuthenticatedRoutesProps = {
  mainRoutes?: ReactNode;
  assetSettingsRoutes?: ReactNode;
  accountSettingsRoutes?: ReactNode;
  extraRoutes?: ReactNode;
};

export function AuthenticatedRoutes(props: AuthenticatedRoutesProps) {
  useSetCurrentAssetFromRoute();

  return (
    <Routes>
      <Route element={<AuthenticatedLayoutTwoColumns />}>
        {props.mainRoutes}
        <Route path={Paths.AssetsLive} element={<AssetLiveTrackingPage />} />
        <Route path={Paths.AssetsLog} element={<AssetLogPage />} />
        <Route path={Paths.AssetsTrips} element={<AssetTripsPage />} />
        <Route path={Paths.AssetsAdd} element={<AssetAddPage />} />
        <Route path={Paths.AssetsReports} element={<AssetReportsPage />} />
        <Route path={Paths.AssetsAlerts} element={<AssetAlertsPage />} />
        <Route path={Paths.Home} element={<HomePage />} />
      </Route>
      <Route element={<AssetSettingsLayout />}>
        {props.assetSettingsRoutes}
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
      </Route>
      <Route element={<AccountSettingsLayout />}>
        {props.accountSettingsRoutes}
        <Route
          path={Paths.SettingsPassword}
          element={<SettingsPasswordPage />}
        />
        <Route path={Paths.SettingsAccount} element={<SettingsAccountPage />} />
      </Route>
      {props.extraRoutes}
      <Route path="*" element={<Navigate replace to={Paths.Home} />} />
    </Routes>
  );
}
