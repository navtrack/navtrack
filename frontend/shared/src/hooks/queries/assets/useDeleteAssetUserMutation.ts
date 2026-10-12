import { useQueryClient } from "@tanstack/react-query";
import {
  getAssetsUsersGetListQueryKey,
  useAssetsUsersDelete
} from "../../../api";

export function useDeleteAssetUserMutation() {
  const queryClient = useQueryClient();

  const mutation = useAssetsUsersDelete({
    mutation: {
      onSuccess: async (_, variables) => {
        await Promise.all([
          queryClient.invalidateQueries({
            queryKey: getAssetsUsersGetListQueryKey(variables.assetId)
          })
        ]);
      }
    }
  });

  return mutation;
}
