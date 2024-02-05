import { faTrashAlt } from "@fortawesome/free-regular-svg-icons";
import { AssetUserModel } from "@navtrack/shared/api/model/generated";
import { useState } from "react";
import { Table, ITableColumn } from "../../../ui/table/Table";
import { DeleteUserFromAssetModal } from "./DeleteUserFromAssetModal";
import { Button } from "../../../ui/button/Button";

type UsersTableProps = {
  rows?: AssetUserModel[];
  loading: boolean;
  refresh: () => void;
};

export function UsersTable(props: UsersTableProps) {
  const [showDeleteModal, setShowDeleteModal] = useState(false);
  const [user, setUser] = useState<AssetUserModel | undefined>();

  const columns: ITableColumn<AssetUserModel>[] = [
    { labelId: "generic.email", render: (user) => user.email },
    { labelId: "generic.role", render: (user) => user.role },
    {
      render: (user) => (
        <Button
          icon={faTrashAlt}
          color="error"
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
