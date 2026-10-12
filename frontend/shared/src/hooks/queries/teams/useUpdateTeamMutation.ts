import { useQueryClient } from "@tanstack/react-query";
import {
  getTeamsGetListQueryKey,
  getTeamsGetQueryKey,
  useTeamsUpdate
} from "../../../api";

export function useUpdateTeamMutation() {
  const queryClient = useQueryClient();

  const mutation = useTeamsUpdate({
    mutation: {
      onSuccess: async (_, variables) => {
        await Promise.all([
          queryClient.invalidateQueries({
            queryKey: getTeamsGetQueryKey(variables.teamId)
          }),
          queryClient.invalidateQueries({
            queryKey: getTeamsGetListQueryKey(variables.teamId)
          })
        ]);
      }
    }
  });

  return mutation;
}
