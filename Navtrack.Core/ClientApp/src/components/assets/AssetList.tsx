import React, { useMemo, useState, useEffect, useCallback } from "react";
import { AssetModel } from "apis/types/asset/AssetModel";
import { useHistory } from "react-router";
import AdminLayout from "components/framework/layouts/admin/AdminLayout";
import { AssetApi } from "apis/AssetApi";
import DeleteModal from "components/framework/DeleteModal";
import { addNotification } from "components/library/notifications/Notifications";
import { NotificationType } from "components/library/notifications/NotificationType";
import Button from "components/library/elements/Button";
import ReactTable from "components/library/table/ReactTable";
import { ApiError } from "framework/httpClient/AppError";

export default function AssetList() {
  const [assets, setAssets] = useState<AssetModel[]>([]);
  const history = useHistory();
  const [showDeleteModal, setShowDeleteModal] = useState(false);
  const [deleteHandler, setHandleDelete] = useState(() => () => {});

  useEffect(() => {
    AssetApi.getAll().then((x) => setAssets(x));
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
        setAssets(devices.filter((x) => x.id !== id));
      })
      .catch((error: ApiError<AssetModel>) => {
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
        Header: "Device Model",
        accessor: (row: any) => row.device.deviceModel.displayName
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
