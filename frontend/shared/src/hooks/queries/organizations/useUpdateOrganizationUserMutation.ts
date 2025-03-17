import { useQueryClient } from "@tanstack/react-query";
import { getUsersListQueryKey, useUsersUpdate } from "../../../api";

export function useUpdateOrganizationUserMutation() {
  const queryClient = useQueryClient();

  const mutation = useUsersUpdate({
    mutation: {
      onSuccess: (_, variables) => {
        return Promise.all([
          queryClient.invalidateQueries({
            queryKey: getUsersListQueryKey(variables.organizationId)
          })
        ]);
      }
    }
  });

  return mutation;
}
