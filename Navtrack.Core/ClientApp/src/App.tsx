import React, { useState, useMemo, useEffect } from "react";
import { Route, Switch, Redirect } from "react-router";
import { AppContextAccessor } from "framework/appContext/AppContextAccessor";
import { AuthenticationService } from "framework/authentication/AuthenticationService";
import AssetEdit from "components/assets/AddAsset";
import UserList from "components/admin/users/UserList";
import UserEdit from "components/admin/users/UserEdit";
import Dashboard from "components/home/Dashboard";
import Login from "components/account/login/Login";
import Register from "components/account/register/Register";
import AppContext, { CreateAppContext, SaveToLocalStorage } from "framework/appContext/AppContext";
import { IntlProvider } from "react-intl";
import translations from "translations";
import AssetLive from "components/assets/live/AssetLive";
import AssetLog from "components/assets/log/AssetLog";
import Notifications from "components/library/notifications/Notifications";
import PrivateRoute from "components/library/routing/PrivateRoute";
import { LocationService } from "services/LocationService";
import { AccountService } from "services/AccountService";
import LoginLayout from "components/framework/layouts/login/LoginLayout";
import AdminLayout from "components/framework/layouts/admin/AdminLayout";
import AssetSettings from "components/assets/settings/AssetSettings";

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
      LocationService.init();
      AccountService.getUserInfo();
    } else {
      LocationService.clear();
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
              <AssetEdit />
            </AdminLayout>
          </PrivateRoute>
          <PrivateRoute path={"/assets/:assetId/live"}>
            <AdminLayout hidePadding={true}>
              <AssetLive />
            </AdminLayout>
          </PrivateRoute>
          <PrivateRoute path={"/assets/:assetId/log"}>
            <AdminLayout hidePadding={true}>
              <AssetLog />
            </AdminLayout>
          </PrivateRoute>
          <PrivateRoute path={"/assets/:assetId/settings"}>
            <AdminLayout hidePadding={true}>
              <AssetSettings />
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
            )}></Route>

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
