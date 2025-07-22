import { useQueryClient } from "@tanstack/react-query";
import {
  getOrganizationsGetQueryKey,
  getOrganizationsUsersListQueryKey,
  useOrganizationsUsersCreate
} from "../../../api";

export function useCreateOrganizationUserMutation() {
  const queryClient = useQueryClient();

  const mutation = useOrganizationsUsersCreate({
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
