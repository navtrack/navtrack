import { FormattedMessage } from "react-intl";
import { AssetUser, AssetUserRole } from "@navtrack/shared/api/model/generated";
import { useDateTime } from "@navtrack/shared/hooks/util/useDateTime";
import { getError } from "@navtrack/shared/utils/api";
import { useCurrentAsset } from "@navtrack/shared/hooks/current/useCurrentAsset";
import { useAssetUsersQuery } from "@navtrack/shared/hooks/queries/assets/useAssetUsersQuery";
import { DeleteModal } from "../../../ui/modal/DeleteModal";
import { ITableColumn } from "../../../ui/table/useTable";
import { Heading } from "../../../ui/heading/Heading";
import { TableV2 } from "../../../ui/table/TableV2";
import { useDeleteAssetUserMutation } from "@navtrack/shared/hooks/queries/assets/useDeleteAssetUserMutation";
import { useNotification } from "../../../ui/notification/useNotification";
import { CreateAssetUserModal } from "./CreateAssetUserModal";
import { useAuthorize } from "@navtrack/shared/hooks/current/useAuthorize";

export function AssetUsersPage() {
  const currentAsset = useCurrentAsset();
  const assetUsers = useAssetUsersQuery({
    assetId: currentAsset.data?.id ?? ""
  });
  const authorize = useAuthorize();

  const deleteUser = useDeleteAssetUserMutation();
  const dateTime = useDateTime();
  const { showNotification } = useNotification();

  const columns: ITableColumn<AssetUser>[] = [
    { labelId: "generic.email", row: (user) => user.email },
    {
      labelId: "assets.settings.access.added-on",
      row: (user) => dateTime.showDate(user.createdDate)
    },
    { labelId: "generic.role", row: (user) => user.userRole },
    {
      rowClassName: "flex justify-end",
      row: (assetUser) => (
        <>
          {authorize.asset(AssetUserRole.Owner) && (
            <DeleteModal
              onConfirm={() => {
                if (currentAsset.data) {
                  return deleteUser.mutateAsync(
                    {
                      assetId: currentAsset.data?.id,
                      userId: assetUser.userId
                    },
                    {
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

  return (
    <>
      <div className="flex justify-between">
        <Heading type="h1">
          <FormattedMessage id="organizations.users.title" />
        </Heading>
        <CreateAssetUserModal />
      </div>
      <TableV2 rows={assetUsers.data?.items} columns={columns} />
    </>
  );
}
