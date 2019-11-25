import React, { useMemo, useState, useEffect } from "react";
import ReactTable from "../Table/ReactTable"
import { useHistory } from "react-router";
import { Link } from "react-router-dom";
import AdminLayout from "../AdminLayout";
import { AssetModel } from "../../services/Api/Model/AssetModel";
import { AssetApi } from "../../services/Api/AssetApi";

export default function AssetList() {
    const [assets, setAssets] = useState<AssetModel[]>([]);
    const history = useHistory();

    useEffect(() => {
        AssetApi.getAll().then(x => setAssets(x));
    }, []);

    const columns = useMemo(() => [
        {
            Header: "Name",
            accessor: "name"
        },
        {
            Header: "Device Type",
            accessor: "deviceType"
        },
        {
            accessor: "id",
            Cell: (cell: any) => <Link to={"/assets/" + cell.cell.value}><i className="fas fa-edit" /></Link>,
            disableSortBy: true
        }
    ], []);

    return (
        <AdminLayout>
            <div className="card shadow">
                <div className="card-header border-0">
                    <div className="row align-items-center">
                        <div className="col">
                            <h3 className="mb-0">Assets</h3>
                        </div>
                        <div className="col text-right">
                            <button className="btn btn-sm btn-primary" onClick={() => history.push("/assets/add")}>Add new asset</button>
                        </div>
                    </div>
                </div>
                <div className="table-responsive">
                    <ReactTable columns={columns} data={assets} />
                </div>
            </div>
        </AdminLayout>
    );
}