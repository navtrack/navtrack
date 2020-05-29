import React from "react";
import AdminLayout from "components/framework/layouts/admin/AdminLayout";

export default function Dashboard() {
    return (
        <AdminLayout>
            <div className="bg-white shadow p-3 rounded flex flex-col">
                <div className="font-bold text-xl">Welcome to Navtrack!</div>
                <div className="mt-2">
                    Thank you for registering so early!
                </div>
                <div>
                    Unfortunately Navtrack is not yet officialy and a lot
                    of things are still not working here, please check back later.
                </div>
            </div>
        </AdminLayout>
    );
}
