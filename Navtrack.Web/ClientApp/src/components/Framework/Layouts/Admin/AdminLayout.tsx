import React from "react";
import AdminSidebar from "./AdminSidebar";
import AdminNavbar from "./AdminNavbar";
import AdminFooter from "./AdminFooter";
import classNames from "classnames";
import { AppError } from "services/HttpClient/AppError";
import Error from "components/Framework/Error";

type Props = {
    children: React.ReactNode
    hidePadding?: boolean,
    error?: AppError
}

export default function AdminLayout(props: Props) {
    return (
        <div className="flex min-h-screen flex-col bg-gray-200">
            <AdminNavbar />
            <div className="flex flex-row flex-grow">
                <AdminSidebar />
                <div className={classNames("flex flex-grow flex-col", { "p-5": !props.hidePadding })}>
                    <Error error={props.error} />
                    {props.children}
                    <AdminFooter className={classNames({ "m-5": props.hidePadding })} />
                </div>
            </div>
        </div>
    );
}