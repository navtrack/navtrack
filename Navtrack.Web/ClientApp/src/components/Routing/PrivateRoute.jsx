import React, { useContext } from "react";
import { Route, Redirect } from "react-router-dom";
import { AppContext } from "../../services/AppContext";

export default function PrivateRoute({ children, ...rest }) {
  const { appContext } = useContext(AppContext);

  return (
    <Route
      {...rest} 
      render={({ location }) =>
        appContext.user.authenticated ? (
          children
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