import { useQueryClient } from "@tanstack/react-query";
import {
  getAssetsUsersGetListQueryKey,
  useAssetsUsersDelete
} from "../../../api";

export function useDeleteAssetUserMutation() {
  const queryClient = useQueryClient();

  const mutation = useAssetsUsersDelete({
    mutation: {
      onSuccess: (_, variables) => {
        return Promise.all([
          queryClient.invalidateQueries({
            queryKey: getAssetsUsersGetListQueryKey(variables.assetId)
          })
        ]);
      }
    }
  });

  return mutation;
}
