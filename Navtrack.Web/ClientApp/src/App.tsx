import React, { useState, useMemo, useEffect } from "react";
import { Route, Switch, Redirect } from "react-router";
import Notifications from "./components/Notifications";
import { CreateAppContext, SaveToLocalStorage } from "./services/AppContext";
import { AppContext } from "./services/AppContext";
import PrivateRoute from "./components/Routing/PrivateRoute";
import { AppContextAccessor } from "./services/AppContext/AppContextAccessor";
import { LocationService } from "services/LocationService";
import { AccountService } from "services/AccountService";
import LiveTracking from "components/Asset/LiveTracking";
import Login from "components/Account/Login";
import Register from "components/Account/Register";
import DeviceList from "components/Settings/Devices/DeviceList";
import DeviceEdit from "components/Settings/Devices/DeviceEdit";
import AssetList from "components/Settings/Assets/AssetList";
import AssetEdit from "components/Settings/Assets/AssetEdit";
import UserList from "components/admin/users/UserList";
import UserEdit from "components/admin/users/UserEdit";
import AssetLog from "components/Asset/Log";
import Dashboard from "components/home/Dashboard";
import { AuthenticationService } from "services/Authentication/AuthenticationService";

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
