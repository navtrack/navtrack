import { useQueryClient } from "@tanstack/react-query";
import {
  getAssetsDevicesGetListQueryKey,
  useAssetsDevicesDelete
} from "../../../api/index-generated";

export function useDeleteDeviceMutation() {
  const queryClient = useQueryClient();

  const mutation = useAssetsDevicesDelete({
    mutation: {
      onSuccess: (_, variables) => {
        return Promise.all([
          queryClient.invalidateQueries({
            queryKey: getAssetsDevicesGetListQueryKey(variables.assetId)
          })
        ]);
      }
    }
  });

  return mutation;
}
