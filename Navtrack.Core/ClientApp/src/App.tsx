import React, { useState, useMemo, useEffect } from "react";
import { Route, Switch, Redirect } from "react-router";
import { AppContextAccessor } from "services/appContext/AppContextAccessor";
import { AuthenticationService } from "services/authentication/AuthenticationService";
import { LocationService } from "services/LocationService";
import { AccountService } from "services/AccountService";
import DeviceList from "components/settings/devices/DeviceList";
import DeviceEdit from "components/settings/devices/DeviceEdit";
import AssetList from "components/settings/assets/AssetList";
import AssetEdit from "components/settings/assets/AssetEdit";
import UserList from "components/admin/users/UserList";
import UserEdit from "components/admin/users/UserEdit";
import Dashboard from "components/home/Dashboard";
import Notifications from "components/framework/notifications/Notifications";
import PrivateRoute from "components/framework/routing/PrivateRoute";
import LiveTracking from "components/asset/liveTracking/LiveTracking";
import Login from "components/account/login/Login";
import Register from "components/account/register/Register";
import AssetLog from "components/asset/log/Log";
import AppContext, { CreateAppContext, SaveToLocalStorage } from "services/appContext/AppContext";

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
      <Notifications />
      <Switch>
        <PrivateRoute path={"/live/:assetId"}>
          <LiveTracking />
        </PrivateRoute>
        <PrivateRoute path={"/log/:assetId"}>
          <AssetLog />
        </PrivateRoute>
        <PrivateRoute exact path="/devices">
          <DeviceList />
        </PrivateRoute>
        <PrivateRoute path="/devices/add">
          <DeviceEdit />
        </PrivateRoute>
        <Route
          path="/devices/:id(\d+)"
          render={props => <DeviceEdit id={props.match.params.id} />}></Route>
        <PrivateRoute exact path="/assets">
          <AssetList />
        </PrivateRoute>
        <PrivateRoute path="/assets/add">
          <AssetEdit />
        </PrivateRoute>
        <Route
          path="/assets/:id"
          render={props => <AssetEdit id={props.match.params.id} />}></Route>
        <PrivateRoute exact path="/admin/users">
          <UserList />
        </PrivateRoute>
        <PrivateRoute path="/admin/users/add">
          <UserEdit />
        </PrivateRoute>
        <Route
          path="/admin/users/:id"
          render={props => <UserEdit id={props.match.params.id} />}></Route>
        <Route path="/login">
          <Login />
        </Route>
        <Route path="/register">
          <Register />
        </Route>
        <PrivateRoute path="/">
          <Dashboard />
        </PrivateRoute>
        <Redirect from="*" to="/" />
      </Switch>
    </AppContext.Provider>
  );
}
