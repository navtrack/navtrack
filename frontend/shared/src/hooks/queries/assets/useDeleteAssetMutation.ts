import { useQueryClient } from "@tanstack/react-query";
import { getAssetsGetListQueryKey, useAssetsDelete } from "../../../api";
import { useCurrentOrganization } from "../../current/useCurrentOrganization";

export function useDeleteAssetMutation() {
  const queryClient = useQueryClient();
  const currentOrganization = useCurrentOrganization();

  const mutation = useAssetsDelete({
    mutation: {
      onSuccess: () => {
        return queryClient.refetchQueries({
          queryKey: getAssetsGetListQueryKey(currentOrganization.id!)
        });
      }
    }
  });

  return mutation;
}
