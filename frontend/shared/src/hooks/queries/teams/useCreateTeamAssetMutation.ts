import { useQueryClient } from "@tanstack/react-query";
import {
  getTeamsAssetsListQueryKey,
  getTeamsGetQueryKey,
  useTeamsAssetsCreate
} from "../../../api";

export function useCreateTeamAssetMutation() {
  const queryClient = useQueryClient();

  const mutation = useTeamsAssetsCreate({
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
