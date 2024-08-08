import { useQueryClient } from "@tanstack/react-query";
import {
  getUserGetQueryKey,
  useAccountChangePassword
} from "../../../api/index-generated";

export function useChangePasswordMutation() {
  const queryClient = useQueryClient();

  const mutation = useAccountChangePassword({
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
