import { useQueryClient } from "@tanstack/react-query";
import {
  getAssetsGetListQueryKey,
  getOrganizationsGetQueryKey,
  getOrganizationsListQueryKey,
  useAssetsCreate
} from "../../../api";

export function useCreateAssetMutation() {
  const queryClient = useQueryClient();

  const mutation = useAssetsCreate({
    mutation: {
      onSuccess: (data, variables) => {
        return Promise.all([
          queryClient.invalidateQueries({
            queryKey: getAssetsGetListQueryKey(variables.organizationId)
          }),
          queryClient.invalidateQueries({
            queryKey: getOrganizationsGetQueryKey(variables.organizationId)
          }),
          queryClient.invalidateQueries({
            queryKey: getOrganizationsListQueryKey()
          })
        ]);
      }
    }
  });

  return mutation;
}
