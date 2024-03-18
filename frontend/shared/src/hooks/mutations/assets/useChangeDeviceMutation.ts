import { useQueryClient } from "@tanstack/react-query";
import {
  getAssetsGetListQueryKey,
  getAssetsGetQueryKey,
  useAssetsDevicesCreateOrUpdate
} from "../../../api/index-generated";

export function useChangeDeviceMutation() {
  const queryClient = useQueryClient();

  const mutation = useAssetsDevicesCreateOrUpdate({
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
