import { useQueryClient } from "@tanstack/react-query";
import {
  getOrganizationsUsersListQueryKey,
  useOrganizationsUsersUpdate
} from "../../../api";

export function useUpdateOrganizationUserMutation() {
  const queryClient = useQueryClient();

  const mutation = useOrganizationsUsersUpdate({
    mutation: {
      onSuccess: (_, variables) => {
        return Promise.all([
          queryClient.invalidateQueries({
            queryKey: getOrganizationsUsersListQueryKey(
              variables.organizationId
            )
          })
        ]);
      }
    }
  });

  return mutation;
}
