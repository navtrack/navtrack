import { useQueryClient } from "@tanstack/react-query";
import { getAssetsGetListQueryKey, useAssetsDelete } from "../../../api";
import { useCurrentOrganization } from "../../current/useCurrentOrganization";

type DeleteAssetMutationProps = {
  onSuccess: () => void;
};

export function useDeleteAssetMutation(props: DeleteAssetMutationProps) {
  const queryClient = useQueryClient();
  const currentOrganization = useCurrentOrganization();

  const mutation = useAssetsDelete({
    mutation: {
      onSuccess: async () => {
        await queryClient.invalidateQueries({
          queryKey: getAssetsGetListQueryKey(currentOrganization.id!)
        });

        props.onSuccess();
      }
    }
  });

  return mutation;
}
