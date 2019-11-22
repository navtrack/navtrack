import React, { Component } from 'react';
import { Route } from 'react-router';
import { FetchData } from './components/FetchData';
import { Counter } from './components/Counter';

import LoginLayout from "./components/LoginLayout";
import Login from "./components/Login";
import AdminLayout from "./components/AdminLayout";
import LiveTracking from "./components/LiveTracking";
import AddDevice from "./components/Devices/Add";
import Home from "./components/Home";

import Devices from "./components/Devices"
import EditDevice from './components/Devices/Edit';

export default class App extends Component {
    static displayName = App.name;

    render() {
        return (
            <>
                <Route path='/live'><AdminLayout><LiveTracking /></AdminLayout></Route>
                <Route path='/counter' component={Counter} />
                <Route path='/fetch-data' component={FetchData} />
                <Route path="/devices/add"><AdminLayout><AddDevice /></AdminLayout></Route>
                <Route path="/devices/:id" render={(props) => <AdminLayout><EditDevice id={props.match.params.id} /></AdminLayout>}></Route>
                <Route exact path="/devices"><AdminLayout><Devices /></AdminLayout></Route>
                <Route path="/assets/add"><AdminLayout><AddDevice /></AdminLayout></Route>
                <Route exact path="/assets"><AdminLayout><Devices /></AdminLayout></Route>
                <Route path="/login"><LoginLayout><Login /></LoginLayout></Route>
                <Route exact path="/"><AdminLayout><Home /></AdminLayout></Route>
            </>
        );
    }
}