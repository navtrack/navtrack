import { useQueryClient } from "@tanstack/react-query";
import {
  getAssetsGetQueryKey,
  getAssetsGetListQueryKey,
  useAssetsUpdate
} from "../../../api/index-generated";
import { useRecoilValue } from "recoil";
import { currentOrganizationIdAtom } from "../../../state/current";

export function useRenameAssetMutation() {
  const queryClient = useQueryClient();
  const currentOrganizationId = useRecoilValue(currentOrganizationIdAtom);

  const mutation = useAssetsUpdate({
    mutation: {
      onSuccess: (_, variables) => {
        return Promise.all([
          queryClient.invalidateQueries({
            queryKey: getAssetsGetQueryKey(variables.assetId)
          }),
          queryClient.invalidateQueries({
            queryKey: getAssetsGetListQueryKey(currentOrganizationId!)
          })
        ]);
      }
    }
  });

  return mutation;
}
