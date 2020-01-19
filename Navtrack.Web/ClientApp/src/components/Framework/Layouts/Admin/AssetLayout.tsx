import React from "react";
import AdminLayout from "./AdminLayout";

type Props = {
    name: string,
    id: number,
    children: React.ReactNode
}

export default function AssetLayout(props: Props) {
    return (
        <AdminLayout>
                <div className="bg-white shadow py-2 px-3 border-bottom">
                    <h5 className="mb-0">{props.name}</h5>
                </div>
                <div className="pl-3 pr-3 pt-3 d-flex flex-column flex-grow-1">
                    {props.children}
                </div>
        </AdminLayout>
    );
}