import { useQueryClient } from "@tanstack/react-query";
import {
  getAssetsUsersGetListQueryKey,
  useAssetsUsersCreate
} from "../../../api";

export function useAssetUserCreateMutation() {
  const queryClient = useQueryClient();

  const mutation = useAssetsUsersCreate({
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
