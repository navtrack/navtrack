import { useQueryClient } from "@tanstack/react-query";
import {
  getTeamsGetQueryKey,
  getTeamsUsersListQueryKey,
  useTeamsUsersCreate
} from "../../../api/index-generated";

export function useCreateTeamUserMutation() {
  const queryClient = useQueryClient();

  const mutation = useTeamsUsersCreate({
    mutation: {
      onSuccess: (_, variables) =>
        Promise.all([
          queryClient.invalidateQueries({
            queryKey: getTeamsGetQueryKey(variables.teamId)
          }),
          queryClient.invalidateQueries({
            queryKey: getTeamsUsersListQueryKey(variables.teamId)
          })
        ])
    }
  });

  return mutation;
}
