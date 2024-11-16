import { useQueryClient } from "@tanstack/react-query";
import {
  getOrganizationsGetQueryKey,
  getTeamsGetListQueryKey,
  useTeamsCreate
} from "../../../api/index-generated";

export function useCreateTeamMutation() {
  const queryClient = useQueryClient();

  const mutation = useTeamsCreate({
    mutation: {
      onSuccess: (_, variables) =>
        Promise.all([
          queryClient.invalidateQueries({
            queryKey: getOrganizationsGetQueryKey(variables.organizationId)
          }),
          queryClient.invalidateQueries({
            queryKey: getTeamsGetListQueryKey(variables.organizationId)
          })
        ])
    }
  });

  return mutation;
}
