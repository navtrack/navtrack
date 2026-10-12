import { useTeamQuery } from "../queries/teams/useTeamQuery";
import { useContext } from "react";
import { CurrentContext } from "./CurrentContext";

export function useCurrentTeam() {
  const current = useContext(CurrentContext);
  const team = useTeamQuery({ teamId: current.teamId });

  return {
    id: current.teamId,
    data: team.data,
    isLoading: team.isLoading
  };
}
