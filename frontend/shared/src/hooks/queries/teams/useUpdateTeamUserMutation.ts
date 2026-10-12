import { useQueryClient } from "@tanstack/react-query";
import { getTeamsUsersListQueryKey, useTeamsUsersUpdate } from "../../../api";

export function useUpdateTeamUserMutation() {
  const queryClient = useQueryClient();

  const mutation = useTeamsUsersUpdate({
    mutation: {
      onSuccess: async (_, variables) => {
        await Promise.all([
          queryClient.invalidateQueries({
            queryKey: getTeamsUsersListQueryKey(variables.teamId)
          })
        ]);
      }
    }
  });

  return mutation;
}
