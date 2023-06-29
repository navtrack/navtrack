import { useQueryClient } from "react-query";
import {
  getGetAssetsAssetIdQueryKey,
  getGetAssetsQueryKey,
  usePatchAssetsAssetId
} from "../../../api/index-generated";

export function useRenameAssetMutation() {
  const queryClient = useQueryClient();

  const mutation = usePatchAssetsAssetId({
    mutation: {
      onSuccess: (_, variables) => {
        return Promise.all([
          queryClient.invalidateQueries({
            queryKey: getGetAssetsAssetIdQueryKey(variables.assetId)
          }),
          queryClient.invalidateQueries({
            queryKey: getGetAssetsQueryKey()
          })
        ]);
      }
    }
  });

  return mutation;
}
