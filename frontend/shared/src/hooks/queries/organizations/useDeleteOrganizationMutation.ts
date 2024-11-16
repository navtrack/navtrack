import { useQueryClient } from "@tanstack/react-query";
import {
  getOrganizationsListQueryKey,
  getUserGetQueryKey,
  useOrganizationsDelete
} from "../../../api/index-generated";

type UseDeleteOrganizationMutationProps = {
  onSuccess?: () => void;
};

export function useDeleteOrganizationMutation(
  props: UseDeleteOrganizationMutationProps
) {
  const queryClient = useQueryClient();

  const mutation = useOrganizationsDelete({
    mutation: {
      onSuccess: (_, variables) => {
        props.onSuccess?.();

        return Promise.all([
          queryClient.refetchQueries({
            queryKey: getOrganizationsListQueryKey()
          }),
          queryClient.refetchQueries({
            queryKey: getUserGetQueryKey()
          })
        ]);
      }
    }
  });

  return mutation;
}
