import { useQueryClient } from "@tanstack/react-query";
import { getUserGetQueryKey, useUserUpdate } from "../../../api";

export function useUpdateUserMutation() {
  const queryClient = useQueryClient();

  const mutation = useUserUpdate({
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
