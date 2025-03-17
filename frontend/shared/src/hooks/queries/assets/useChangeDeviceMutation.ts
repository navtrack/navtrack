import { useQueryClient } from "@tanstack/react-query";
import {
  getAssetsDevicesGetListQueryKey,
  getAssetsGetQueryKey,
  getAssetsGetListQueryKey,
  useAssetsDevicesCreateOrUpdate
} from "../../../api";
import { useCurrentOrganization } from "../../current/useCurrentOrganization";

export function useChangeDeviceMutation() {
  const queryClient = useQueryClient();
  const currentOrganization = useCurrentOrganization();

  const mutation = useAssetsDevicesCreateOrUpdate({
    mutation: {
      onSuccess: (_, variables) => {
        return Promise.all([
          queryClient.invalidateQueries({
            queryKey: getAssetsGetQueryKey(variables.assetId)
          }),
          queryClient.invalidateQueries({
            queryKey: getAssetsGetListQueryKey(currentOrganization.id!)
          }),
          queryClient.invalidateQueries({
            queryKey: getAssetsDevicesGetListQueryKey(variables.assetId)
          })
        ]);
      }
    }
  });

  return mutation;
}
