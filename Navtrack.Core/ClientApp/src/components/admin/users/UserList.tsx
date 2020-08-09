import React, { useMemo, useState, useEffect, useCallback } from "react";
import { UserModel } from "apis/types/user/UserModel";
import { useHistory } from "react-router";
import { UserApi } from "apis/UserApi";
import DeleteModal from "components/framework/DeleteModal";
import { addNotification } from "components/library/notifications/Notifications";
import Button from "components/library/elements/Button";
import ReactTable from "components/library/table/ReactTable";
import { ApiError } from "framework/httpClient/AppError";

export default function UserList() {
  const [users, setUsers] = useState<UserModel[]>([]);
  const history = useHistory();
  const [showDeleteModal, setShowDeleteModal] = useState(false);
  const [deleteHandler, setHandleDelete] = useState(() => () => {});

  useEffect(() => {
    UserApi.getAll().then((x) => setUsers(x));
  }, []);

  const handleDeleteClick = useCallback(
    (id: number) => {
      setShowDeleteModal(true);
      setHandleDelete(() => () => deleteUser(id, users));
    },
    [users]
  );

  const deleteUser = (id: number, users: UserModel[]) => {
    UserApi.delete(id)
      .then(() => {
        addNotification("User deleted successfully.");
        setUsers(users.filter((x) => x.id !== id));
      })
      .catch((error: ApiError<UserModel>) => {
        addNotification(`${error.message}`);
      });
  };

  const columns = useMemo(
    () => [
      {
        Header: "Email",
        accessor: "email"
      },
      {
        Header: "Actions",
        accessor: "id",
        Cell: (cell: any) => (
          <>
            <i
              className="fas fa-edit mr-3 hover:text-gray-700 cursor-pointer"
              onClick={() => history.push(`/admin/users/${cell.cell.value}`)}
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
    <>
      {showDeleteModal && (
        <DeleteModal closeModal={() => setShowDeleteModal(false)} deleteHandler={deleteHandler} />
      )}
      <div className="shadow rounded bg-white flex flex-col">
        <div className="p-3 flex">
          <div className="flex-grow font-medium text-lg">Users</div>
          <div className="flex-grow flex justify-end">
            <Button color="primary" onClick={() => history.push("/admin/users/add")}>
              Add user
            </Button>
          </div>
        </div>
        <ReactTable columns={columns} data={users} />
      </div>
    </>
  );
}
