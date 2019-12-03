import React, { useState, useMemo, useEffect } from 'react';
import { Route, Switch, Redirect } from 'react-router';

import Login from "./components/Login";
import LiveTracking from "./components/LiveTracking";
import Home from "./components/Home";

import DeviceList from "./components/Devices/DeviceList"
import DeviceEdit from './components/Devices/DeviceEdit';

import AssetList from "./components/Assets/AssetList"
import AssetEdit from './components/Assets/AssetEdit';
import IMEIGenerator from "./components/IMEIGenerator";
import Notifications from './components/Notifications';
import { AppContext, GetAppContext, SetUserInLocalStorage } from './services/AppContext';
import PrivateRoute from "./components/Routing/PrivateRoute";
import { AppContextAccessor } from "./services/AppContext/AppContextAccessor";

export default function App() {
    const [appContext, setAppContext] = useState(GetAppContext());
    AppContextAccessor.setNewSetAppContext(setAppContext);
    const appContextWrapper = useMemo(() => ({ appContext, setAppContext }), [appContext, setAppContext])

    useEffect(() => {
        AppContextAccessor.setNewAppContext(appContext);

        SetUserInLocalStorage(appContext.user);
    }, [appContext]);


    return (
        <AppContext.Provider value={appContextWrapper}>
            <Notifications />
            <Switch>
                <PrivateRoute path='/imei'><IMEIGenerator /></PrivateRoute>

                <PrivateRoute path='/live'><LiveTracking /></PrivateRoute>

                <PrivateRoute exact path="/devices"><DeviceList /></PrivateRoute>
                <PrivateRoute path="/devices/add"><DeviceEdit /></PrivateRoute>
                <Route path="/devices/:id(\d+)" render={(props) => <DeviceEdit id={props.match.params.id} />}></Route>

                <PrivateRoute exact path="/assets"><AssetList /></PrivateRoute>
                <PrivateRoute path="/assets/add"><AssetEdit /></PrivateRoute>
                <Route path="/assets/:id" render={(props) => <AssetEdit id={props.match.params.id} />}></Route>

                <Route path="/login"><Login /></Route>
                <PrivateRoute path="/"><Home /></PrivateRoute>

                <Redirect from="*" to="/" />
            </Switch>
        </AppContext.Provider >
    );
}