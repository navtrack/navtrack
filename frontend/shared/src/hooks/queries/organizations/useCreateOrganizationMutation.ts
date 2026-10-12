import { useQueryClient } from "@tanstack/react-query";
import {
  getOrganizationsListQueryKey,
  getUserGetQueryKey,
  useOrganizationsCreate
} from "../../../api";

export function useCreateOrganizationMutation() {
  const queryClient = useQueryClient();

  const mutation = useOrganizationsCreate({
    mutation: {
      onSuccess: async (_, variables) => {
        await Promise.all([
          queryClient.invalidateQueries({
            queryKey: getUserGetQueryKey()
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
