import { useMatch } from "react-router-dom";
import { useTeamQuery } from "../queries/teams/useTeamQuery";

export function useCurrentTeam() {
  const match = useMatch("/teams/:id/*");
  const team = useTeamQuery({ teamId: match?.params.id });

  return {
    id: match?.params.id,
    data: team.data,
    isLoading: team.isLoading
  };
}
