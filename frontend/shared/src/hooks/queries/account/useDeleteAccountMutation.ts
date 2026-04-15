import { useQueryClient } from "@tanstack/react-query";
import { useAccountDelete } from "../../../api";

type UseDeleteAccountMutationProps = {
  onSuccess?: () => void;
};

export function useDeleteAccountMutation(
  props?: UseDeleteAccountMutationProps
) {
  const queryClient = useQueryClient();
  const mutation = useAccountDelete({
    mutation: {
      onSuccess: async () => {
        await queryClient.clear();

        props?.onSuccess?.();
      }
    }
  });

  return mutation;
}
