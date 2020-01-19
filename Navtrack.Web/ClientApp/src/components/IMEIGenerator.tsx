import React from "react";
import AdminLayout from "./Framework/Layouts/Admin/AdminLayout";

type Props = {
    id?: number
}

export default function IMEIGenerator(props: Props) {
    const imei = Math.floor((Math.random() * 1000000000000000) + 100000000000000);

    return (
        <AdminLayout>
            <div className="card shadow">
                <div className="card-header">
                    <div className="row align-items-center">
                        <div className="col">
                            <h3 className="mb-0">{imei}</h3>
                        </div>
                    </div>
                </div>
            </div>
        </AdminLayout>
    );
}