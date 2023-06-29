import { useQueryClient } from "react-query";
import {
  getGetAssetsAssetIdQueryKey,
  getGetAssetsQueryKey,
  usePostAssetsAssetIdDevices
} from "../../../api/index-generated";

export function useChangeDeviceMutation() {
  const queryClient = useQueryClient();

  const mutation = usePostAssetsAssetIdDevices({
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
