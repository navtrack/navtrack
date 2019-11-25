import React, { Component } from 'react';
import { Route, Switch } from 'react-router';
import { FetchData } from './components/FetchData';
import { Counter } from './components/Counter';

import LoginLayout from "./components/LoginLayout";
import Login from "./components/Login";
import AdminLayout from "./components/AdminLayout";
import LiveTracking from "./components/LiveTracking";
import Home from "./components/Home";

import DeviceList from "./components/Devices/DeviceList"
import DeviceEdit from './components/Devices/DeviceEdit';

import AssetList from "./components/Assets/AssetList"
import AssetEdit from './components/Assets/AssetEdit';

export default class App extends Component {
    static displayName = App.name;

    render() {
        return (
            <>
                <Switch>
                    <Route path='/live'><AdminLayout><LiveTracking /></AdminLayout></Route>
                    <Route path='/counter' component={Counter} />
                    <Route path='/fetch-data' component={FetchData} />

                    <Route exact path="/devices"><DeviceList /></Route>
                    <Route path="/devices/add"><DeviceEdit /></Route>
                    <Route path="/devices/:id" render={(props) => <DeviceEdit id={props.match.params.id} />}></Route>

                    <Route exact path="/assets"><AssetList /></Route>
                    <Route path="/assets/add"><AssetEdit /></Route>
                    <Route path="/assets/:id" render={(props) => <AssetEdit id={props.match.params.id} />}></Route>

                    <Route path="/login"><LoginLayout><Login /></LoginLayout></Route>
                    <Route exact path="/"><AdminLayout><Home /></AdminLayout></Route>
                </Switch>

            </>
        );
    }
}