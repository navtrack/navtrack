import React, { useMemo, useState, useEffect, useCallback } from "react";
import { AssetModel } from "../../services/Api/Model/AssetModel";
import { useHistory } from "react-router";
import { AssetApi } from "../../services/Api/AssetApi";
import { addNotification } from "../Notifications";
import { ApiError } from "../../services/HttpClient/HttpClient";
import { Link } from "react-router-dom";
import DeleteModal from "../Common/DeleteModal";
import ReactTable from "../Table/ReactTable";
import AdminLayout from "components/Framework/Layouts/Admin/AdminLayout";


export default function AssetList() {
    const [assets, setAssets] = useState<AssetModel[]>([]);
    const history = useHistory();
    const [showDeleteModal, setShowDeleteModal] = useState(false);
    const [deleteHandler, setHandleDelete] = useState(() => () => { });

    useEffect(() => {
        AssetApi.getAll().then(x => setAssets(x));
    }, []);

    const handleDeleteClick = useCallback((id: number) => {
        setShowDeleteModal(true);
        setHandleDelete(() => () => deleteAsset(id, assets));
    }, [assets]);

    const deleteAsset = (id: number, devices: AssetModel[]) => {
        AssetApi.delete(id)
            .then(() => {
                addNotification("Asset deleted successfully.");
                setAssets(devices.filter(x => x.id !== id));
            })
            .catch((error: ApiError) => {
                addNotification(error.message);
            });
    }

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
            Header: "Actions",
            accessor: "id",
            Cell: (cell: any) =>
                <div>
                    <Link to={"/assets/" + cell.cell.value} className="mx-2"><i className="fas fa-edit" /></Link>
                    <span className="btn-link" onClick={() => handleDeleteClick(cell.cell.value)}><i className="fas fa-trash" /></span>
                </div>,
            disableSortBy: true
        }
    ], [handleDeleteClick]);

    return (
        <AdminLayout>
            <DeleteModal show={showDeleteModal} setShow={setShowDeleteModal} deleteHandler={deleteHandler} 
            description="All the location history of this asset will also be removed." />
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