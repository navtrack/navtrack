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
      onSuccess: (_, variables) => {
        return Promise.all([
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
