import { useQueryClient } from "@tanstack/react-query";
import { getUserGetQueryKey, useUserUpdate } from "../../../api";

export function useUpdateUserMutation() {
  const queryClient = useQueryClient();

  const mutation = useUserUpdate({
    mutation: {
      onSuccess: () => {
        return queryClient.refetchQueries({
          queryKey: getUserGetQueryKey()
        });
      }
    }
  });

  return mutation;
}
