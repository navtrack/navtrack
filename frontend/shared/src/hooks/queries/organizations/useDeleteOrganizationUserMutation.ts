import { useQueryClient } from "@tanstack/react-query";
import {
  getOrganizationsGetQueryKey,
  getUsersListQueryKey,
  useUsersDelete
} from "../../../api/index-generated";

export function useDeleteOrganizationUserMutation() {
  const queryClient = useQueryClient();

  const mutation = useUsersDelete({
    mutation: {
      onSuccess: (_, variables) =>
        Promise.all([
          queryClient.invalidateQueries({
            queryKey: getUsersListQueryKey(variables.organizationId)
          }),
          queryClient.invalidateQueries({
            queryKey: getOrganizationsGetQueryKey(variables.organizationId)
          })
        ])
    }
  });

  return mutation;
}
