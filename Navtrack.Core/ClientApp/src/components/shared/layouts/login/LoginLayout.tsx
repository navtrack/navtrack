import React from "react";
import { Switch, Route, Redirect } from "react-router";
import Login from "../../../account/login/Login";
import Register from "../../../account/register/Register";

export default function LoginLayout() {
  return (
    <div className="bg-gray-800 flex min-h-screen items-center justify-center flex-col">
      <div className="max-w-xs w-full flex flex-col items-center">
        <div className="h-16 m-3 ">
          <a href="https://www.navtrack.io">
            <img src="/navtrack.png" width="64" className="mb-4" alt="Navtrack" />
          </a>
        </div>
        <Switch>
          <Route path="/login">
            <Login />
          </Route>
          <Route path="/register">
            <Register />
          </Route>
          <Redirect from="*" to="/login" />
        </Switch>
      </div>
    </div>
  );
}
