import React, { useState, useMemo, useEffect } from "react";
import { Route, Switch, Redirect } from "react-router";
import AppContext, {
  CreateAppContext,
  SaveToLocalStorage
} from "./framework/appContext/AppContext";
import AssetSettingsLayout from "./components/assets/settings/AssetSettingsLayout";
import UserEdit from "./components/admin/users/UserEdit";
import AddAsset from "./components/assets/AddAsset";
import AssetLive from "./components/assets/live/AssetLive";
import Dashboard from "./components/home/Dashboard";
import Login from "./components/account/login/Login";
import AssetLog from "./components/assets/log/AssetLog";
import AdminLayout from "./components/framework/layouts/admin/AdminLayout";
import Register from "./components/account/register/Register";
import { AppContextAccessor } from "./framework/appContext/AppContextAccessor";
import translations from "./translations";
import { AuthenticationService } from "./framework/authentication/AuthenticationService";
import { AccountService } from "./services/AccountService";
import Notifications from "./components/library/notifications/Notifications";
import PrivateRoute from "./components/library/routing/PrivateRoute";
import LoginLayout from "./components/framework/layouts/login/LoginLayout";
import UserList from "./components/admin/users/UserList";
import { AssetsService } from "./services/AssetService";
import { IntlProvider } from "react-intl";
import DeviceLayout from "./components/devices/DeviceLayout";

export default function App() {
  const [appContext, setAppContext] = useState(CreateAppContext());
  AppContextAccessor.setAppContextSetter(setAppContext);
  AppContextAccessor.setAppContextGetter(() => appContext);
  const appContextWrapper = useMemo(() => ({ appContext, setAppContext }), [
    appContext,
    setAppContext
  ]);

  useEffect(() => {
    SaveToLocalStorage(appContext);
  }, [appContext]);

  useEffect(() => {
    (async () => {
      await AuthenticationService.checkAndRenewAccessToken();
    })();
  }, []);

  useEffect(() => {
    if (appContext.authenticationInfo.authenticated) {
      AssetsService.init();
      AccountService.getUserInfo();
    } else {
      AssetsService.clear();
    }
  }, [appContext.authenticationInfo]);

  return (
    <AppContext.Provider value={appContextWrapper}>
      <IntlProvider locale="en" messages={translations["en"]}>
        <Notifications />
        <Switch>
          {/* Assets */}
          <PrivateRoute path="/assets/add">
            <AdminLayout>
              <AddAsset />
            </AdminLayout>
          </PrivateRoute>
          <PrivateRoute path={"/assets/:assetId/live"}>
            <AdminLayout>
              <AssetLive />
            </AdminLayout>
          </PrivateRoute>
          <PrivateRoute path={"/assets/:assetId/log"}>
            <AdminLayout>
              <AssetLog />
            </AdminLayout>
          </PrivateRoute>
          <PrivateRoute path={"/assets/:assetId/settings"}>
            <AdminLayout>
              <AssetSettingsLayout />
            </AdminLayout>
          </PrivateRoute>

          {/*Devices*/}
          <PrivateRoute path={"/devices/:deviceId"}>
            <AdminLayout>
              <DeviceLayout />
            </AdminLayout>
          </PrivateRoute>

          {/* Login */}
          <Route path="/login">
            <LoginLayout>
              <Login />
            </LoginLayout>
          </Route>
          <Route path="/register">
            <LoginLayout>
              <Register />
            </LoginLayout>
          </Route>

          {/* Users */}
          <PrivateRoute exact path="/admin/users">
            <AdminLayout>
              <UserList />
            </AdminLayout>
          </PrivateRoute>
          <PrivateRoute path="/admin/users/add">
            <AdminLayout>
              <UserEdit />
            </AdminLayout>
          </PrivateRoute>
          <Route
            path="/admin/users/:id"
            render={(props) => (
              <AdminLayout>
                <UserEdit id={props.match.params.id} />
              </AdminLayout>
            )}
          />

          {/* Home */}
          <PrivateRoute path="/">
            <AdminLayout>
              <Dashboard />
            </AdminLayout>
          </PrivateRoute>
          <Redirect from="*" to="/" />
        </Switch>
      </IntlProvider>
    </AppContext.Provider>
  );
}
