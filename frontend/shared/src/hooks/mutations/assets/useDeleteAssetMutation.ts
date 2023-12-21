import { useQueryClient } from "@tanstack/react-query";
import {
  getAssetsGetListQueryKey,
  useAssetsDelete
} from "../../../api/index-generated";

export function useDeleteAssetMutation() {
  const queryClient = useQueryClient();

  const mutation = useAssetsDelete({
    mutation: {
      onSuccess: () => {
        return queryClient.refetchQueries({
          queryKey: getAssetsGetListQueryKey()
        });
      }
    }
  });

  return mutation;
}
