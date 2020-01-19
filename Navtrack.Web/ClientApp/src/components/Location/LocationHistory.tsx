import React, { useMemo, useState, useEffect } from "react";
import ReactTable from "../Table/ReactTable"
import { LocationApi } from "../../services/Api/LocationApi";
import { LocationModel } from "../../services/Api/Model/LocationModel";
import AdminLayout from "components/Framework/Layouts/Admin/AdminLayout";

export default function LocationHistory() {
    const [devices, setHistory] = useState<LocationModel[]>([]);

    useEffect(() => {
        LocationApi.getHistory(6).then(x => setHistory(x));
    }, []);


    const columns = useMemo(() => [
        {
            Header: "Latitude",
            accessor: "latitude"
        },
        {
            Header: "Longitude",
            accessor: "longitude"
        },
        {
            Header: "Date",
            accessor: "dateTime"
        }
    ], []);

    return (
        <AdminLayout>
            <div className="card shadow">
                <div className="card-header border-0">
                    <div className="row align-items-center">
                        <div className="col">
                            <h3 className="mb-0">History</h3>
                        </div>
                    </div>
                </div>
                <div className="table-responsive">
                    <ReactTable columns={columns} data={devices} />
                </div>
            </div>
        </AdminLayout>
    );
}