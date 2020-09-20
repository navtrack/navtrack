import React from "react";
import { Switch, Route, Redirect } from "react-router";
import { ApiError } from "../../../../services/httpClient/AppError";
import AddAsset from "../../../assets/AddAsset";
import Dashboard from "../../../home/Dashboard";
import ErrorCard from "../../error/ErrorCard";
import PrivateRoute from "../../routing/PrivateRoute";
import AssetLayout from "../../../assets/AssetLayout";
import AdminFooter from "./AdminFooter";
import AdminMap from "./AdminMap";
import AdminNavbar from "./AdminNavbar";
import AdminSidebar from "./AdminSidebar";

type Props = {
  error?: ApiError<object>;
};

export default function AdminLayout(props: Props) {
  return (
    <>
      <AdminMap />
      <div className="flex min-h-screen flex-col bg-gray-100" style={{ minWidth: "800px" }}>
        <AdminNavbar />
        <div className="flex flex-row flex-grow">
          <AdminSidebar />
          <div className="flex flex-grow flex-col">
            <ErrorCard error={props.error} />
            <div className="flex flex-grow flex-col">
              <div id="admin-content" className="z-10">
                <Switch>
                  {/* Assets */}
                  <PrivateRoute path="/assets/add">
                    <AddAsset />
                  </PrivateRoute>
                  <Route path={"/assets/:assetId/"}>
                    <AssetLayout />
                  </Route>

                  {/* Home */}
                  <PrivateRoute path="/">
                    <Dashboard />
                  </PrivateRoute>
                  <Redirect from="*" to="/" />
                </Switch>
              </div>
            </div>
            <AdminFooter />
          </div>
        </div>
      </div>
    </>
  );
}
