import React, { useContext, ReactNode } from "react";
import { UserRole } from "apis/types/user/UserRole";
import AppContext from "framework/appContext/AppContext";
import { Route, Redirect } from "react-router";
import { AuthorizationService } from "framework/authentication/AuthorizationService";
import AdminLayout from "components/framework/layouts/admin/AdminLayout";

type Props = {
  children: ReactNode;
  path: string;
  exact?: boolean;
  userRole?: UserRole;
};

export default function PrivateRoute(props: Props) {
  const { appContext } = useContext(AppContext);

  return (
    <Route
      path={props.path}
      exact={props.exact}
      render={({ location }) =>
        appContext.authenticationInfo.authenticated ? (
          AuthorizationService.isAuthorized(props.userRole) ? (
            props.children
          ) : (
            <AdminLayout error={{ status: 401 }}>
              <></>
            </AdminLayout>
          )
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
