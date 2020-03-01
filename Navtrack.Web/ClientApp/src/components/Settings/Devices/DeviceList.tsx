import React, { useMemo, useState, useEffect, useCallback } from "react";
import { DeviceModel } from "services/Api/Model/DeviceModel";
import { useHistory } from "react-router";
import { DeviceApi } from "services/Api/DeviceApi";
import { addNotification } from "components/Notifications";
import { AppError } from "services/HttpClient/AppError";
import { Link } from "react-router-dom";
import AdminLayout from "components/Framework/Layouts/Admin/AdminLayout";
import DeleteModal from "components/Common/DeleteModal";
import ReactTable from "components/Table/ReactTable";
import Button from "components/Framework/Elements/Button";

export default function DeviceList() {
  const [devices, setDevices] = useState<DeviceModel[]>([]);
  const history = useHistory();
  const [showDeleteModal, setShowDeleteModal] = useState(false);
  const [deleteHandler, setHandleDelete] = useState(() => () => {});

  useEffect(() => {
    DeviceApi.getAll().then(x => setDevices(x));
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
        setDevices(devices.filter(x => x.id !== id));
      })
      .catch((error: AppError) => {
        addNotification(`${error.message}`);
      });
  };

  const columns = useMemo(
    () => [
      {
        Header: "Name",
        accessor: "name"
      },
      {
        Header: "Type",
        accessor: "type"
      },
      {
        Header: "IMEI",
        accessor: "imei"
      },
      {
        Header: "Actions",
        accessor: "id",
        Cell: (cell: any) => (
          <div>
            <Link to={"/devices/" + cell.cell.value} className="mx-2">
              <i className="fas fa-edit" />
            </Link>
            <span className="btn-link" onClick={() => handleDeleteClick(cell.cell.value)}>
              <i className="fas fa-trash" />
            </span>
          </div>
        ),
        disableSortBy: true
      }
    ],
    [handleDeleteClick]
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
