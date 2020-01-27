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

            <div className="shadow rounded bg-white flex flex-col">
                <div className="p-4 flex">
                    <div className="flex-grow font-medium text-lg">Assets</div>
                    <div className="flex-grow flex justify-end">
                        <button className="shadow-md bg-gray-800 hover:bg-gray-700 text-white text-sm py-1 px-4 rounded focus:outline-none" onClick={() => history.push("/assets/add")}>
                            Add asset
                        </button>
                    </div>
                </div>
                <ReactTable columns={columns} data={assets} />
            </div>
        </AdminLayout>
    );
}