import React from "react";
import { Switch, Route, Redirect } from "react-router";
import LoginPage from "../../../login/LoginPage";
import Register from "../../../register/RegisterPage";
import NavtrackLogo from "../../../../assets/images/navtrack.svg";
import { useIntl } from "react-intl";

export default function LoginLayout() {
  const intl = useIntl();

  return (
    <div className="bg-gray-900 flex min-h-screen flex-col">
      <div className="justify-center flex flex-1 items-end">
        <a href="https://www.navtrack.io" className="mb-4">
          <img
            src={NavtrackLogo}
            width="100"
            height="100"
            alt={intl.formatMessage({ id: "navtrack" })}
          />
        </a>
      </div>
      <div className="flex justify-center px-4">
        <div className="flex w-full max-w-md">
          <Switch>
            <Route path="/login">
              <LoginPage />
            </Route>
            <Route path="/register">
              <Register />
            </Route>
            <Redirect from="*" to="/login" />
          </Switch>
        </div>
      </div>
      <div className="flex-1">
        <div style={{ height: "100px" }} className="mt-4"></div>
      </div>
    </div>
  );
}
