import { useQueryClient } from "@tanstack/react-query";
import {
  getAssetsDevicesGetListQueryKey,
  useAssetsDevicesDelete
} from "../../../api";

export function useDeleteDeviceMutation() {
  const queryClient = useQueryClient();

  const mutation = useAssetsDevicesDelete({
    mutation: {
      onSuccess: async (_, variables) => {
        await Promise.all([
          queryClient.invalidateQueries({
            queryKey: getAssetsDevicesGetListQueryKey(variables.assetId)
          })
        ]);
      }
    }
  });

  return mutation;
}
