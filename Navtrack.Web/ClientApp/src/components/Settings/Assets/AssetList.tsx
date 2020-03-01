import React, { useMemo, useState, useEffect, useCallback } from "react";
import AdminLayout from "components/Framework/Layouts/Admin/AdminLayout";
import { AssetModel } from "services/Api/Model/AssetModel";
import { useHistory } from "react-router";
import { AssetApi } from "services/Api/AssetApi";
import { addNotification } from "components/Notifications";
import { AppError } from "services/HttpClient/AppError";
import DeleteModal from "components/Common/DeleteModal";
import ReactTable from "components/Table/ReactTable";
import Button from "components/Framework/Elements/Button";
import { NotificationType } from "components/Notifications/NotificationType";

export default function AssetList() {
  const [assets, setAssets] = useState<AssetModel[]>([]);
  const history = useHistory();
  const [showDeleteModal, setShowDeleteModal] = useState(false);
  const [deleteHandler, setHandleDelete] = useState(() => () => {});

  useEffect(() => {
    AssetApi.getAll().then(x => setAssets(x));
  }, []);

  const handleDeleteClick = useCallback(
    (id: number) => {
      setShowDeleteModal(true);
      setHandleDelete(() => () => deleteAsset(id, assets));
    },
    [assets]
  );

  const deleteAsset = (id: number, devices: AssetModel[]) => {
    AssetApi.delete(id)
      .then(() => {
        addNotification("Asset deleted successfully.");
        setAssets(devices.filter(x => x.id !== id));
      })
      .catch((error: AppError) => {
        addNotification(`${error.message}`, NotificationType.Error);
      });
  };

  const columns = useMemo(
    () => [
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
        Cell: (cell: any) => (
          <>
            <i
              className="fas fa-edit mr-3 hover:text-gray-700 cursor-pointer"
              onClick={() => history.push(`/assets/${cell.cell.value}`)}
            />
            <i
              className="fas fa-trash hover:text-gray-700 cursor-pointer"
              onClick={() => handleDeleteClick(cell.cell.value)}
            />
          </>
        ),
        disableSortBy: true
      }
    ],
    [handleDeleteClick, history]
  );

  return (
    <AdminLayout>
      {showDeleteModal && (
        <DeleteModal
          description="All the location history of this asset will also be removed."
          closeModal={() => setShowDeleteModal(false)}
          deleteHandler={deleteHandler}
        />
      )}
      <div className="shadow rounded bg-white flex flex-col">
        <div className="p-3 flex">
          <div className="flex-grow font-medium text-lg">Assets</div>
          <div className="flex-grow flex justify-end">
            <Button color="primary" onClick={() => history.push("/assets/add")}>
              Add asset
            </Button>
          </div>
        </div>
        <ReactTable columns={columns} data={assets} />
      </div>
    </AdminLayout>
  );
}
