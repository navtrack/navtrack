import { useQueryClient } from "@tanstack/react-query";
import {
  getAssetsGetListQueryKey,
  useAssetsCreate
} from "../../../api/index-generated";

export function useAddAssetMutation() {
  const queryClient = useQueryClient();

  const mutation = useAssetsCreate({
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
