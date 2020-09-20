import React from "react";
import ReactDOM from "react-dom";
import App from "./App";

import "tailwindcss/dist/base.min.css";
import "tailwindcss/dist/components.min.css";
import "tailwindcss/dist/utilities.min.css";
import "@fortawesome/fontawesome-free/css/all.min.css";
import "leaflet/dist/leaflet.css";
import "./app.css";

const rootElement = document.getElementById("app-root");

ReactDOM.render(<App />, rootElement);
