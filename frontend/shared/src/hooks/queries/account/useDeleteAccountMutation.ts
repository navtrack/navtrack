import { useQueryClient } from "@tanstack/react-query";
import { useAccountDelete } from "../../../api";
import { ErrorModel } from "../../../api/model";

type UseDeleteAccountMutationProps = {
  onSuccess?: () => void;
  onError?: (error: ErrorModel) => void;
};

export function useDeleteAccountMutation(
  props?: UseDeleteAccountMutationProps
) {
  const queryClient = useQueryClient();
  const mutation = useAccountDelete({
    mutation: {
      onSuccess: async () => {
        queryClient.clear();

        props?.onSuccess?.();
      },
      onError: (error) => props?.onError?.(error)
    }
  });

  return mutation;
}
