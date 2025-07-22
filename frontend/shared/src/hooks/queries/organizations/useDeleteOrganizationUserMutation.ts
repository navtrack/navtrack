import { useQueryClient } from "@tanstack/react-query";
import {
  getOrganizationsGetQueryKey,
  getOrganizationsUsersListQueryKey,
  useOrganizationsUsersDelete
} from "../../../api";

export function useDeleteOrganizationUserMutation() {
  const queryClient = useQueryClient();

  const mutation = useOrganizationsUsersDelete({
    mutation: {
      onSuccess: (_, variables) =>
        Promise.all([
          queryClient.invalidateQueries({
            queryKey: getOrganizationsUsersListQueryKey(
              variables.organizationId
            )
          }),
          queryClient.invalidateQueries({
            queryKey: getOrganizationsGetQueryKey(variables.organizationId)
          })
        ])
    }
  });

  return mutation;
}
