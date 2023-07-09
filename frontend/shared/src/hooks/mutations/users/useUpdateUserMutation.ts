import { useQueryClient } from "react-query";
import { getGetUserQueryKey, usePatchUser } from "../../../api/index-generated";

export function useUpdateUserMutation() {
  const queryClient = useQueryClient();

  const mutation = usePatchUser({
    mutation: {
      onSuccess: (_, variables) => {
        return queryClient.refetchQueries({
          queryKey: getGetUserQueryKey()
        });
      }
    }
  });

  return mutation;
}
