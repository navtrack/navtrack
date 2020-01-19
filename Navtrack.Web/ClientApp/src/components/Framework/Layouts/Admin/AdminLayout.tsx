import React from "react";
import AdminSidebar from "./AdminSidebar";
import AdminNavbar from "./AdminNavbar";
import AdminFooter from "./AdminFooter";

type Props = {
    children: React.ReactNode
}

export default function AdminLayout(props: Props) {
    return (
        <div className="d-flex vh-100 flex-column">
            <div className="bg-primary">
                <AdminNavbar />
            </div>
            <div className="d-flex flex-row flex-fill">
                <AdminSidebar />
                <div className="d-flex flex-fill flex-column">
                    {props.children}
                    <div className="p-3">
                        <AdminFooter />
                    </div>
                </div>
            </div>
        </div>
    );
}