import { useQueryClient } from "@tanstack/react-query";
import {
  getTeamsAssetsListQueryKey,
  getTeamsGetQueryKey,
  useTeamsAssetsDelete
} from "../../../api/index-generated";

export function useDeleteTeamAssetMutation() {
  const queryClient = useQueryClient();

  const mutation = useTeamsAssetsDelete({
    mutation: {
      onSuccess: (_, variables) =>
        Promise.all([
          queryClient.invalidateQueries({
            queryKey: getTeamsAssetsListQueryKey(variables.teamId)
          }),
          queryClient.invalidateQueries({
            queryKey: getTeamsGetQueryKey(variables.teamId)
          })
        ])
    }
  });

  return mutation;
}
