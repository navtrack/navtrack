import React from "react";
import AdminSidebar from "./AdminSidebar";
import AdminNavbar from "./AdminNavbar";
import AdminFooter from "./AdminFooter";
import Error from "components/library/error/Error";
import { ApiError } from "framework/httpClient/AppError";

type Props = {
  children: React.ReactNode;
  error?: ApiError<object>;
};

export default function AdminLayout(props: Props) {
  return (
    <div className="flex min-h-screen flex-col bg-gray-100" style={{ minWidth: "800px" }}>
      <AdminNavbar />
      <div className="flex flex-row flex-grow">
        <AdminSidebar />
        <div className="flex flex-grow flex-col">
          <Error error={props.error} />
          <div className="flex flex-grow flex-col">{props.children}</div>
          <AdminFooter />
        </div>
      </div>
    </div>
  );
}
