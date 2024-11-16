import { FormattedMessage } from "react-intl";
import { Heading } from "../ui/heading/Heading";
import { useTeamsQuery } from "@navtrack/shared/hooks/queries/teams/useTeamsQuery";
import { ITableColumn } from "../ui/table/useTable";
import { Team } from "@navtrack/shared/api/model/generated";
import { CreateTeamModal } from "./CreateTeamModal";
import { generatePath, Link, useNavigate } from "react-router-dom";
import { Paths } from "../../app/Paths";
import { TableV2 } from "../ui/table/TableV2";
import { useCurrentOrganization } from "@navtrack/shared/hooks/current/useCurrentOrganization";

export function TeamsPage() {
  const currentOrganization = useCurrentOrganization();
  const teams = useTeamsQuery({ organizationId: currentOrganization.data?.id });

  const columns: ITableColumn<Team>[] = [
    {
      labelId: "generic.name",
      row: (team) => (
        <Link
          to={generatePath(Paths.TeamUsers, { id: team.id })}
          className="text-blue-700">
          {team.name}
        </Link>
      )
    },
    { labelId: "generic.users", row: (team) => team.usersCount },
    { labelId: "generic.assets", row: (team) => team.assetsCount }
  ];

  const navigate = useNavigate();

  return (
    <>
      <div className="flex justify-between">
        <Heading type="h1">
          <FormattedMessage id="organizations.teams.title" />
        </Heading>
        <CreateTeamModal />
      </div>
      <TableV2
        rows={teams.data?.items}
        columns={columns}
        rowClick={(row) =>
          navigate(generatePath(Paths.TeamUsers, { id: row.id }))
        }
      />
    </>
  );
}
