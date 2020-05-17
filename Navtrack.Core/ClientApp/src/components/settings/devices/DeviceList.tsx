import React, { useMemo, useState, useEffect, useCallback } from "react";
import { DeviceModel } from "services/api/types/device/DeviceModel";
import { useHistory } from "react-router";
import { DeviceApi } from "services/api/DeviceApi";
import { addNotification } from "components/framework/notifications/Notifications";
import { AppError } from "services/httpClient/AppError";
import AdminLayout from "components/framework/layouts/admin/AdminLayout";
import DeleteModal from "components/common/DeleteModal";
import Button from "components/framework/elements/Button";
import ReactTable from "components/framework/table/ReactTable";

export default function DeviceList() {
  const [devices, setDevices] = useState<DeviceModel[]>([]);
  const history = useHistory();
  const [showDeleteModal, setShowDeleteModal] = useState(false);
  const [deleteHandler, setHandleDelete] = useState(() => () => {});

  useEffect(() => {
    DeviceApi.getAll().then((x) => setDevices(x));
  }, []);

  const handleDeleteClick = useCallback(
    (id: number) => {
      setShowDeleteModal(true);
      setHandleDelete(() => () => deleteDevice(id, devices));
    },
    [devices]
  );

  const deleteDevice = (id: number, devices: DeviceModel[]) => {
    DeviceApi.delete(id)
      .then(() => {
        addNotification("Device deleted successfully.");
        setDevices(devices.filter((x) => x.id !== id));
      })
      .catch((error: AppError) => {
        addNotification(`${error.message}`);
      });
  };

  const columns = useMemo(
    () => [
      {
        Header: "ID",
        accessor: "deviceId"
      },
      {
        Header: "Model",
        accessor: (row: any) => row.deviceModel.displayName
      },
      {
        Header: "Actions",
        accessor: "id",
        Cell: (cell: any) => (
          <>
            <i
              className="fas fa-edit mr-3 hover:text-gray-700 cursor-pointer"
              onClick={() => history.push(`/devices/${cell.cell.value}`)}
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
        <DeleteModal closeModal={() => setShowDeleteModal(false)} deleteHandler={deleteHandler} />
      )}
      <div className="shadow rounded bg-white flex flex-col">
        <div className="p-3 flex">
          <div className="flex-grow font-medium text-lg">Devices</div>
          <div className="flex-grow flex justify-end">
            <Button color="primary" onClick={() => history.push("/devices/add")}>
              Add device
            </Button>
          </div>
        </div>
        <ReactTable columns={columns} data={devices} />
      </div>
    </AdminLayout>
  );
}
