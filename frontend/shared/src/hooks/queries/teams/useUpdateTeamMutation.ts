import { useQueryClient } from "@tanstack/react-query";
import {
  getTeamsGetListQueryKey,
  getTeamsGetQueryKey,
  useTeamsUpdate
} from "../../../api/index-generated";

export function useUpdateTeamMutation() {
  const queryClient = useQueryClient();

  const mutation = useTeamsUpdate({
    mutation: {
      onSuccess: (_, variables) => {
        return Promise.all([
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
