import {
  AssetRoleType,
  AssetUserModel
} from "@navtrack/shared/api/model/generated";
import { ITableColumn } from "../../../ui/table/useTable";
import { TableV1 } from "../../../ui/table/TableV1";
import { useDateTime } from "@navtrack/shared/hooks/util/useDateTime";
import { DeleteModal } from "../../../ui/modal/DeleteModal";
import { FormattedMessage } from "react-intl";
import { useCurrentAsset } from "@navtrack/shared/hooks/assets/useCurrentAsset";
import { useAssetUserDeleteMutation } from "@navtrack/shared/hooks/mutations/assets/useAssetUserDeleteMutation";
import { getError } from "@navtrack/shared/utils/api";
import { useNotification } from "../../../ui/notification/useNotification";

type AssetUsersTableProps = {
  rows?: AssetUserModel[];
  loading: boolean;
  refresh: () => void;
};

export function AssetUsersTable(props: AssetUsersTableProps) {
  const currentAsset = useCurrentAsset();
  const deleteUser = useAssetUserDeleteMutation();
  const dateTime = useDateTime();
  const { showNotification } = useNotification();

  const columns: ITableColumn<AssetUserModel>[] = [
    { labelId: "generic.email", row: (user) => user.email },
    {
      labelId: "assets.settings.access.added-on",
      row: (user) => dateTime.showDate(user.createdDate)
    },
    { labelId: "generic.role", row: (user) => user.role },
    {
      rowClassName: "flex justify-end",
      row: (assetUser) => (
        <>
          {assetUser.role !== AssetRoleType.Owner && (
            <DeleteModal
              onConfirm={() => {
                if (currentAsset.data) {
                  return deleteUser.mutateAsync(
                    {
                      assetId: currentAsset.data?.id,
                      userId: assetUser.userId
                    },
                    {
                      onSuccess: () => {
                        props.refresh();
                      },
                      onError: (error) => {
                        const model = getError(error);

                        showNotification({
                          type: "error",
                          description: `${model.message}`
                        });
                      }
                    }
                  );
                }
              }}>
              <FormattedMessage
                id="assets.settings.access.delete-user.question"
                values={{
                  email: <span className="font-bold">{assetUser.email}</span>,
                  assetName: (
                    <span className="font-bold">{currentAsset.data?.name}</span>
                  )
                }}
              />
            </DeleteModal>
          )}
        </>
      )
    }
  ];

  return <TableV1 rows={props.rows} columns={columns} />;
}
