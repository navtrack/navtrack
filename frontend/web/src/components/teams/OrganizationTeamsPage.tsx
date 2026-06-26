import { FormattedMessage } from "react-intl";
import { Heading } from "../ui/heading/Heading";
import { useTeamsQuery } from "@navtrack/shared/hooks/queries/teams/useTeamsQuery";
import { ITableColumn, useTable } from "../ui/table/useTable";
import { TeamModel } from "@navtrack/shared/api/model";
import { CreateTeamModal } from "./CreateTeamModal";
import { generatePath, Link, useNavigate } from "react-router-dom";
import { Paths } from "../../app/Paths";
import { TableV2 } from "../ui/table/TableV2";
import { useCurrentOrganization } from "@navtrack/shared/hooks/current/useCurrentOrganization";

export function OrganizationTeamsPage() {
  const currentOrganization = useCurrentOrganization();
  const teams = useTeamsQuery({ organizationId: currentOrganization.data?.id });

  const columns: ITableColumn<TeamModel>[] = [
    {
      labelId: "name",
      row: (team) => (
        <Link
          to={generatePath(Paths.TeamUsers, { id: team.id })}
          className="text-blue-700">
          {team.name}
        </Link>
      )
    },
    { labelId: "users", row: (team) => team.usersCount },
    { labelId: "assets", row: (team) => team.assetsCount }
  ];

  const navigate = useNavigate();
  const table = useTable({ rows: teams.data?.items, columns });

  return (
    <>
      <div className="flex justify-between">
        <Heading type="h1">
          <FormattedMessage id="organizations.teams.title" />
        </Heading>
        <CreateTeamModal />
      </div>
      <TableV2
        {...table.props}
        rowClickHandler={(row) =>
          navigate(generatePath(Paths.TeamUsers, { id: row.id }))
        }
      />
    </>
  );
}
