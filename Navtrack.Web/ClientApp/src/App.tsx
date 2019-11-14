import React, {Component} from 'react';
import {Route} from 'react-router';
import {Home} from './components/Home';
import {FetchData} from './components/FetchData';
import {Counter} from './components/Counter';

import './custom.css'
import LoginLayout from "./components/LoginLayout";
import Login from "./components/Login";

export default class App extends Component {
    static displayName = App.name;

    render() {
        return (
            <>
                <Route path='/counter' component={Counter}/>
                <Route path='/fetch-data' component={FetchData}/>
                <Route path="/login"><LoginLayout><Login/></LoginLayout></Route>
                <Route exact path='/' component={Home}/>
            </>
        );
    }
}
