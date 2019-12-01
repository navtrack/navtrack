import React from 'react';
import { Route, Switch } from 'react-router';

import Login from "./components/Login";
import AdminLayout from "./components/Layouts/Admin/AdminLayout";
import LiveTracking from "./components/LiveTracking";
import Home from "./components/Home";

import DeviceList from "./components/Devices/DeviceList"
import DeviceEdit from './components/Devices/DeviceEdit';

import AssetList from "./components/Assets/AssetList"
import AssetEdit from './components/Assets/AssetEdit';
import IMEIGenerator from "./components/IMEIGenerator";
import Notifications from './components/Notifications';

export default function App() {

    return (
        <>
            <Notifications />
            <Switch>
                <Route path='/imei'><IMEIGenerator /></Route>

                <Route path='/live'><AdminLayout><LiveTracking /></AdminLayout></Route>

                <Route exact path="/devices"><DeviceList /></Route>
                <Route path="/devices/add"><DeviceEdit /></Route>
                <Route path="/devices/:id(\d+)" render={(props) => <DeviceEdit id={props.match.params.id} />}></Route>

                <Route exact path="/assets"><AssetList /></Route>
                <Route path="/assets/add"><AssetEdit /></Route>
                <Route path="/assets/:id" render={(props) => <AssetEdit id={props.match.params.id} />}></Route>

                <Route path="/login"><Login /></Route>
                <Route exact path="/"><AdminLayout><Home /></AdminLayout></Route>
            </Switch>
        </>
    );
}