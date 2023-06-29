import { useQueryClient } from "react-query";
import {
  getGetAssetsQueryKey,
  usePostAssets
} from "../../../api/index-generated";

export function useAddAssetMutation() {
  const queryClient = useQueryClient();

  const mutation = usePostAssets({
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
