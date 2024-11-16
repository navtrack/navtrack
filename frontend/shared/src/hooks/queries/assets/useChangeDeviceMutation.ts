import { useQueryClient } from "@tanstack/react-query";
import {
  getAssetsDevicesGetListQueryKey,
  getAssetsGetQueryKey,
  getAssetsGetListQueryKey,
  useAssetsDevicesCreateOrUpdate
} from "../../../api/index-generated";
import { useRecoilValue } from "recoil";
import { currentOrganizationIdAtom } from "../../../state/current";

export function useChangeDeviceMutation() {
  const queryClient = useQueryClient();
  const currentOrganizationId = useRecoilValue(currentOrganizationIdAtom);

  const mutation = useAssetsDevicesCreateOrUpdate({
    mutation: {
      onSuccess: (_, variables) => {
        return Promise.all([
          queryClient.invalidateQueries({
            queryKey: getAssetsGetQueryKey(variables.assetId)
          }),
          queryClient.invalidateQueries({
            queryKey: getAssetsGetListQueryKey(currentOrganizationId!)
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
