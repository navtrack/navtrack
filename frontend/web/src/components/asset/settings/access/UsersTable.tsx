import { faTrashAlt } from "@fortawesome/free-regular-svg-icons";
import { AssetUserModel } from "@navtrack/ui-shared/api/model/generated";
import { useState } from "react";
import IconButton from "../../../ui/shared/button/IconButton";
import Table, { ITableColumn } from "../../../ui/shared/table/Table";
import DeleteUserFromAssetModal from "./DeleteUserFromAssetModal";

interface IUsersTable {
  rows?: AssetUserModel[];
  loading: boolean;
  refresh: () => void;
}

export default function UsersTable(props: IUsersTable) {
  const [showDeleteModal, setShowDeleteModal] = useState(false);
  const [user, setUser] = useState<AssetUserModel | undefined>();

  const columns: ITableColumn<AssetUserModel>[] = [
    { labelId: "generic.email", render: (user) => user.email },
    { labelId: "generic.role", render: (user) => user.role },
    {
      render: (user) => (
        <IconButton
          icon={faTrashAlt}
          className="text-red-500"
          onClick={() => {
            setUser(user);
            setShowDeleteModal(true);
          }}
        />
      )
    }
  ];

  return (
    <>
      <Table rows={props.rows} loading={props.loading} columns={columns} />
      <DeleteUserFromAssetModal
        user={user}
        show={showDeleteModal}
        refresh={props.refresh}
        close={() => setShowDeleteModal(false)}
      />
    </>
  );
}
