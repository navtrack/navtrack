import { useQueryClient } from "@tanstack/react-query";
import {
  getOrganizationsGetQueryKey,
  getOrganizationsListQueryKey,
  useOrganizationsUpdate
} from "../../../api";

export function useUpdateOrganizationMutation() {
  const queryClient = useQueryClient();

  const mutation = useOrganizationsUpdate({
    mutation: {
      onSuccess: async (_, variables) => {
        await Promise.all([
          queryClient.invalidateQueries({
            queryKey: getOrganizationsGetQueryKey(variables.organizationId)
          }),
          queryClient.invalidateQueries({
            queryKey: getOrganizationsListQueryKey()
          })
        ]);
      }
    }
  });

  return mutation;
}
