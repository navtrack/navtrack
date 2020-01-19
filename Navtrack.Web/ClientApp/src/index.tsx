import React from 'react';
import ReactDOM from 'react-dom';
import { BrowserRouter } from 'react-router-dom';
import App from './App';
import registerServiceWorker from './registerServiceWorker';

import "./assets/js/plugins/nucleo/css/nucleo.css";
import "./assets/js/plugins/@fortawesome/fontawesome-free/css/all.min.css";
import "./assets/css/custom.css";

import "font-awesome/css/font-awesome.min.css";
import "bootstrap/dist/css/bootstrap.css";
import "./assets/css/argon-dashboard.css";
import "./custom.css";

const rootElement = document.getElementById('root');

ReactDOM.render(
  <BrowserRouter>
    <App />
  </BrowserRouter>,
  rootElement);

registerServiceWorker();