import { Heading } from "../../ui/heading/Heading";
import { ITableColumn, useTable } from "../../ui/table/useTable";
import { TeamUserModel } from "@navtrack/shared/api/model";
import { generatePath, Link, useParams } from "react-router-dom";
import { Paths } from "../../../app/Paths";
import { TableV2 } from "../../ui/table/TableV2";
import { TeamLayout } from "../TeamLayout";
import { useTeamUsersQuery } from "@navtrack/shared/hooks/queries/teams/useTeamUsersQuery";
import { DeleteModal } from "../../ui/modal/DeleteModal";
import { useNotification } from "../../ui/notification/useNotification";
import { FormattedMessage } from "react-intl";
import { useDeleteTeamUserMutation } from "@navtrack/shared/hooks/queries/teams/useDeleteTeamUserMutation";
import { useShow } from "@navtrack/shared/hooks/util/useShow";
import { useTeamQuery } from "@navtrack/shared/hooks/queries/teams/useTeamQuery";
import { CreateTeamUserModal } from "./CreateTeamUserModal";
import { UpdateTeamUserModal } from "./UpdateTeamUserModal";
import { Authorize } from "@navtrack/shared/components/authorize/Authorize";
import { formatErrorMessage } from "@navtrack/shared/utils/errors";

export function TeamUsersPage() {
  const { id } = useParams();
  const users = useTeamUsersQuery({ teamId: id });
  const { showNotification } = useNotification();
  const deleteUser = useDeleteTeamUserMutation();
  const show = useShow();
  const team = useTeamQuery({ teamId: id });

  const columns: ITableColumn<TeamUserModel>[] = [
    {
      labelId: "email",
      row: (user) => (
        <Link
          to={generatePath(Paths.TeamUsers, { id: user.userId })}
          className="text-blue-700">
          {user.email}
        </Link>
      )
    },
    { labelId: "role", row: (user) => user.userRole },
    {
      labelId: "added-on",
      row: (user) => show.date(user.createdDate)
    },
    {
      rowClassName: "flex justify-end space-x-2",
      row: (user) => (
        <Authorize teamUserRole="Owner">
          <UpdateTeamUserModal user={user} teamId={team.data?.id} />
          <DeleteModal
            isLoading={deleteUser.isPending}
            onConfirm={(close) => {
              if (team.data) {
                return deleteUser.mutateAsync(
                  {
                    teamId: team.data.id,
                    userId: user.userId
                  },
                  {
                    onSuccess: () => {
                      showNotification({
                        type: "success",
                        description: "teams.users.delete.success"
                      });
                    },
                    onError: (error) => {
                      showNotification({
                        type: "error",
                        description: formatErrorMessage(error.title)
                      });
                    }
                  }
                );
              }
            }}>
            <FormattedMessage
              id="teams.users.delete-question"
              values={{
                user: <span className="font-bold">{user.email}</span>,
                team: <span className="font-bold">{team.data?.name}</span>
              }}
            />
          </DeleteModal>
        </Authorize>
      )
    }
  ];
  const table = useTable({ rows: users.data?.items, columns });

  return (
    <TeamLayout team={team.data} isLoading={team.isLoading}>
      <div className="flex justify-between">
        <Heading type="h1">
          <FormattedMessage id="manage-users" />
        </Heading>
        <Authorize teamUserRole="Owner">
          <CreateTeamUserModal teamId={id} />
        </Authorize>
      </div>
      <TableV2 {...table.props} />
    </TeamLayout>
  );
}
