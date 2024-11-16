import { useQueryClient } from "@tanstack/react-query";
import {
  getOrganizationsGetQueryKey,
  getUsersListQueryKey,
  useUsersCreate
} from "../../../api/index-generated";

export function useCreateOrganizationUserMutation() {
  const queryClient = useQueryClient();

  const mutation = useUsersCreate({
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
