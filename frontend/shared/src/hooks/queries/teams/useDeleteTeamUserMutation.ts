import { useQueryClient } from "@tanstack/react-query";
import {
  getTeamsGetQueryKey,
  getTeamsUsersListQueryKey,
  useTeamsUsersDelete
} from "../../../api/index-generated";

export function useDeleteTeamUserMutation() {
  const queryClient = useQueryClient();

  const mutation = useTeamsUsersDelete({
    mutation: {
      onSuccess: (_, variables) => {
        return Promise.all([
          queryClient.invalidateQueries({
            queryKey: getTeamsGetQueryKey(variables.teamId)
          }),
          queryClient.invalidateQueries({
            queryKey: getTeamsUsersListQueryKey(variables.teamId)
          })
        ]);
      }
    }
  });

  return mutation;
}
