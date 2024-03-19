import { faTrashAlt } from "@fortawesome/free-regular-svg-icons";
import { AssetUserModel } from "@navtrack/shared/api/model/generated";
import { useState } from "react";
import { DeleteUserFromAssetModal } from "./DeleteUserFromAssetModal";
import { Button } from "../../../ui/button/Button";
import { ITableColumn } from "../../../ui/table/useTable";
import { TableV1 } from "../../../ui/table/TableV1";

type UsersTableProps = {
  rows?: AssetUserModel[];
  loading: boolean;
  refresh: () => void;
};

export function UsersTable(props: UsersTableProps) {
  const [showDeleteModal, setShowDeleteModal] = useState(false);
  const [user, setUser] = useState<AssetUserModel | undefined>();

  const columns: ITableColumn<AssetUserModel>[] = [
    { labelId: "generic.email", row: (user) => user.email },
    { labelId: "generic.role", row: (user) => user.role },
    {
      row: (user) => (
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
      <TableV1 rows={props.rows} columns={columns} />
      <DeleteUserFromAssetModal
        user={user}
        show={showDeleteModal}
        refresh={props.refresh}
        close={() => setShowDeleteModal(false)}
      />
    </>
  );
}
