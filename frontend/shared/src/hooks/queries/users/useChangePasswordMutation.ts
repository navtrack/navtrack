import { useQueryClient } from "@tanstack/react-query";
import { getUserGetQueryKey, useUserChangePassword } from "../../../api";

export function useChangePasswordMutation() {
  const queryClient = useQueryClient();

  const mutation = useUserChangePassword({
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
