import React, { useContext, ReactNode } from "react";
import { Route, Redirect } from "react-router-dom";
import { AppContext } from "../../services/AppContext";

type Props = {
  children: ReactNode;
  path: string;
  exact?: boolean;
};
export default function PrivateRoute(props: Props) {
  const { appContext } = useContext(AppContext);

  return (
    <Route
      path={props.path}
      exact={props.exact}
      render={({ location }) =>
        appContext.authenticationInfo.authenticated ? (
          props.children
        ) : (
          <Redirect
            to={{
              pathname: "/login",
              state: { from: location }
            }}
          />
        )
      }
    />
  );
}
