import { useQueryClient } from "@tanstack/react-query";
import {
  getAssetsGetListQueryKey,
  useAssetsDelete
} from "../../../api/index-generated";
import { useRecoilValue } from "recoil";
import { currentOrganizationIdAtom } from "../../../state/current";

export function useDeleteAssetMutation() {
  const queryClient = useQueryClient();
  const currentOrganizationId = useRecoilValue(currentOrganizationIdAtom);

  const mutation = useAssetsDelete({
    mutation: {
      onSuccess: () => {
        return queryClient.refetchQueries({
          queryKey: getAssetsGetListQueryKey(currentOrganizationId!)
        });
      }
    }
  });

  return mutation;
}
