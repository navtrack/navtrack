import { FormattedMessage } from "react-intl";
import { CreateUserModal } from "./CreateUserModal";
import { useCurrentUserQuery } from "@navtrack/shared/hooks/queries/user/useCurrentUserQuery";
import { Heading } from "../ui/heading/Heading";
import { OrganizationUser } from "@navtrack/shared/api/model/generated";
import { useDateTime } from "@navtrack/shared/hooks/util/useDateTime";
import { getError } from "@navtrack/shared/utils/api";
import { DeleteModal } from "../ui/modal/DeleteModal";
import { useNotification } from "../ui/notification/useNotification";
import { ITableColumn } from "../ui/table/useTable";
import { UpdateUserModal } from "./UpdateUserModal";
import { useDeleteOrganizationUserMutation } from "@navtrack/shared/hooks/queries/organizations/useDeleteOrganizationUserMutation";
import { TableV2 } from "../ui/table/TableV2";
import { useCurrentOrganization } from "@navtrack/shared/hooks/current/useCurrentOrganization";
import { useOrganizationUsersQuery } from "@navtrack/shared/hooks/queries/organizations/useOrganizationUsersQuery";

export function UsersPage() {
  const currentUser = useCurrentUserQuery();
  const currentOrganization = useCurrentOrganization();
  const organizationUsers = useOrganizationUsersQuery({
    organizationId: currentOrganization.data?.id
  });

  const deleteUser = useDeleteOrganizationUserMutation();
  const dateTime = useDateTime();
  const { showNotification } = useNotification();

  const columns: ITableColumn<OrganizationUser>[] = [
    { labelId: "generic.email", row: (user) => user.email },
    {
      labelId: "generic.added-on",
      row: (user) => dateTime.showDate(user.createdDate)
    },
    { labelId: "generic.role", row: (user) => user.userRole },
    {
      rowClassName: "flex justify-end space-x-2",
      row: (user) => (
        <>
          <UpdateUserModal user={user} />
          <DeleteModal
            isLoading={deleteUser.isLoading}
            onConfirm={(close) => {
              if (currentUser.data && currentOrganization.data) {
                return deleteUser.mutateAsync(
                  {
                    organizationId: currentOrganization.data.id,
                    userId: user.userId
                  },
                  {
                    onSuccess: () => {
                      showNotification({
                        type: "success",
                        description: "organization.users.delete.success"
                      });
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
              id="organizations.users.delete-question"
              values={{
                user: <span className="font-bold">{user.email}</span>,
                organization: (
                  <span className="font-bold">
                    {currentOrganization.data?.name}
                  </span>
                )
              }}
            />
          </DeleteModal>
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
        <CreateUserModal />
      </div>
      <TableV2 rows={organizationUsers.data?.items} columns={columns} />
    </>
  );
}
