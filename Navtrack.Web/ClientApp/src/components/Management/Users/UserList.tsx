import React, { useMemo, useState, useEffect, useCallback } from "react";
import { UserModel } from "services/Api/Model/UserModel";
import { useHistory } from "react-router";
import { UserApi } from "services/Api/UserApi";
import { addNotification } from "components/Notifications";
import { AppError } from "services/HttpClient/AppError";
import { Link } from "react-router-dom";
import AdminLayout from "components/Framework/Layouts/Admin/AdminLayout";
import DeleteModal from "components/Common/DeleteModal";
import ReactTable from "components/Table/ReactTable";

export default function UserList() {
  const [users, setUsers] = useState<UserModel[]>([]);
  const history = useHistory();
  const [showDeleteModal, setShowDeleteModal] = useState(false);
  const [deleteHandler, setHandleDelete] = useState(() => () => { });

  useEffect(() => {
    UserApi.getAll().then(x => setUsers(x));
  }, []);

  const handleDeleteClick = useCallback((id: number) => {
    setShowDeleteModal(true);
    setHandleDelete(() => () => deleteUser(id, users));
  }, [users]);

  const deleteUser = (id: number, users: UserModel[]) => {
    UserApi.delete(id)
      .then(() => {
        addNotification("User deleted successfully.");
        setUsers(users.filter(x => x.id !== id));

      })
      .catch((error: AppError) => {
        addNotification(`${error.message}`);
      });
  }

  const columns = useMemo(() => [
    {
      Header: "Email",
      accessor: "email"
    },
    {
      Header: "Actions",
      accessor: "id",
      Cell: (cell: any) =>
        <div>
          <Link to={"/users/" + cell.cell.value} className="mx-2"><i className="fas fa-edit" /></Link>
          <span className="btn-link" onClick={() => handleDeleteClick(cell.cell.value)}><i className="fas fa-trash" /></span>
        </div>,
      disableSortBy: true
    }
  ], [handleDeleteClick]);

  return (

    <AdminLayout>
      <DeleteModal show={showDeleteModal} setShow={setShowDeleteModal} deleteHandler={deleteHandler} />
      <div className="shadow rounded bg-white flex flex-col">
        <div className="p-4 flex">
          <div className="flex-grow font-medium text-lg">Users</div>
          <div className="flex-grow flex justify-end">
            <button className="shadow-md bg-gray-800 hover:bg-gray-700 text-white text-sm py-1 px-4 rounded focus:outline-none"
              onClick={() => history.push("/users/add")}>
              Add user
            </button>
          </div>
        </div>
        <ReactTable columns={columns} data={users} />
      </div>
    </AdminLayout>
  );
}