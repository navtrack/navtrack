import { useQueryClient } from "@tanstack/react-query";
import {
  getAssetsGetListQueryKey,
  getAssetsGetQueryKey,
  useAssetsUpdate
} from "../../../api/index-generated";

export function useRenameAssetMutation() {
  const queryClient = useQueryClient();

  const mutation = useAssetsUpdate({
    mutation: {
      onSuccess: (_, variables) => {
        return Promise.all([
          queryClient.invalidateQueries({
            queryKey: getAssetsGetQueryKey(variables.assetId)
          }),
          queryClient.invalidateQueries({
            queryKey: getAssetsGetListQueryKey()
          })
        ]);
      }
    }
  });

  return mutation;
}
