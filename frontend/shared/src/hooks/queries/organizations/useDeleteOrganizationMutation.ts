import { useQueryClient } from "@tanstack/react-query";
import {
  getOrganizationsListQueryKey,
  getUserGetQueryKey,
  useOrganizationsDelete
} from "../../../api";

type UseDeleteOrganizationMutationProps = {
  onSuccess?: () => void;
};

export function useDeleteOrganizationMutation(
  props: UseDeleteOrganizationMutationProps
) {
  const queryClient = useQueryClient();

  const mutation = useOrganizationsDelete({
    mutation: {
      onSuccess: async () => {
        await Promise.all([
          queryClient.invalidateQueries({ queryKey: getUserGetQueryKey() }),
          queryClient.invalidateQueries({
            queryKey: getOrganizationsListQueryKey()
          })
        ]);

        props.onSuccess?.();
      }
    }
  });

  return mutation;
}
