import { useQueryClient } from "@tanstack/react-query";
import { getUserGetQueryKey, useUserChangePassword } from "../../../api";

export function useChangePasswordMutation() {
  const queryClient = useQueryClient();

  const mutation = useUserChangePassword({
    mutation: {
      onSuccess: async () => {
        await queryClient.invalidateQueries({
          queryKey: getUserGetQueryKey()
        });
      }
    }
  });

  return mutation;
}
