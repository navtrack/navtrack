import { useQueryClient } from "react-query";
import {
  getGetAssetsQueryKey,
  useDeleteAssetsAssetId
} from "../../../api/index-generated";

export function useDeleteAssetMutation() {
  const queryClient = useQueryClient();

  const mutation = useDeleteAssetsAssetId({
    mutation: {
      onSuccess: () => {
        return queryClient.refetchQueries({
          queryKey: getGetAssetsQueryKey()
        });
      }
    }
  });

  return mutation;
}
