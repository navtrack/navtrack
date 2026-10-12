import { useQueryClient } from "@tanstack/react-query";
import {
  getAssetsUsersGetListQueryKey,
  useAssetsUsersCreate
} from "../../../api";

export function useAssetUserCreateMutation() {
  const queryClient = useQueryClient();

  const mutation = useAssetsUsersCreate({
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
