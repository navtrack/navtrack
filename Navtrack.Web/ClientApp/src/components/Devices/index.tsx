import React, { useMemo, useState, useEffect } from "react";
import ReactTable from "../Table/ReactTable"
import { useHistory } from "react-router";
import { Device } from "../../services/Api/Types/Device";
import { DeviceApi } from "../../services/Api/DeviceApi";
import { Link } from "react-router-dom";

export default function Devices() {
    const [devices, setDevices] = useState<Device[]>([]);

    const history = useHistory();

    useEffect(() => {
        DeviceApi.getAll().then(x => setDevices(x));
    }, []);


    const columns = useMemo(() => [
        {
            Header: "IMEI",
            accessor: "imei",
            Cell: (cell: any) => <Link to={"/devices/"+ cell.row.original.id}>{cell.cell.value}</Link>,
        },
        {
            Header: "Name",
            accessor: "name"
        },
        {
            accessor: "id",
            Cell: (cell: any) => <Link to={"/devices/"+ cell.cell.value}><i className="fas fa-edit" /></Link>,
            disableSortBy: true
        }
    ], []);

    return (
        <>
            <div className="card shadow">
                <div className="card-header border-0">
                    <div className="row align-items-center">
                        <div className="col">
                            <h3 className="mb-0">Devices</h3>
                        </div>
                        <div className="col text-right">
                            <button className="btn btn-sm btn-primary" onClick={() => history.push("/devices/add")}>Add new device</button>
                        </div>
                    </div>
                </div>
                <div className="table-responsive">
                    <ReactTable columns={columns} data={devices} />
                </div>
            </div>
        </>
    );
}