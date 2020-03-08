import React, { useContext, ReactNode } from "react";
import { Route, Redirect } from "react-router-dom";
import { AppContext } from "../../services/AppContext";
import { UserRole } from "services/Api/Model/UserRole";
import { AuthorizationService } from "services/Authentication/AuthorizationService";
import AdminLayout from "components/framework/Layouts/Admin/AdminLayout";

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
            <AdminLayout error={{ status: 401, validationResult: {}, message: "" }}>
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
