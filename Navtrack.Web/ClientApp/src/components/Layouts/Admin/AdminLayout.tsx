import React from "react";
import AdminSidebar from "./AdminSidebar";
import AdminNavbar from "./AdminNavbar";
import AdminFooter from "./AdminFooter";

type Props = {
    children: React.ReactNode
}

export default function AdminLayout(props: Props) {
    return (
        <>
            <AdminSidebar />
            <div className="main-content">
                <AdminNavbar />

                <div className="container-fluid p-4">
                    {props.children}
                </div>

                <div className="container-fluid p-4">
                    <AdminFooter />
                </div>
            </div>
        </>
    );
}