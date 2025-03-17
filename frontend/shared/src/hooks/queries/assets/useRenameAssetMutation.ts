import { useQueryClient } from "@tanstack/react-query";
import {
  getAssetsGetQueryKey,
  getAssetsGetListQueryKey,
  useAssetsUpdate
} from "../../../api";
import { useCurrentOrganization } from "../../current/useCurrentOrganization";

export function useRenameAssetMutation() {
  const queryClient = useQueryClient();
  const currentOrganization = useCurrentOrganization();

  const mutation = useAssetsUpdate({
    mutation: {
      onSuccess: (_, variables) => {
        return Promise.all([
          queryClient.invalidateQueries({
            queryKey: getAssetsGetQueryKey(variables.assetId)
          }),
          queryClient.invalidateQueries({
            queryKey: getAssetsGetListQueryKey(currentOrganization.id!)
          })
        ]);
      }
    }
  });

  return mutation;
}
