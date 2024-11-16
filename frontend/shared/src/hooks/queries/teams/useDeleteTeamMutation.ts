import { useQueryClient } from "@tanstack/react-query";
import {
  getOrganizationsGetQueryKey,
  getTeamsAssetsListQueryKey,
  useTeamsDelete
} from "../../../api/index-generated";

type DeleteTeamMutationProps = {
  organizationId?: string;
};

export function useDeleteTeamMutation(props: DeleteTeamMutationProps) {
  const queryClient = useQueryClient();

  const mutation = useTeamsDelete({
    mutation: {
      onSuccess: (_, variables) =>
        Promise.all([
          queryClient.invalidateQueries({
            queryKey: getTeamsAssetsListQueryKey(variables.teamId)
          }),
          queryClient.invalidateQueries({
            queryKey: getOrganizationsGetQueryKey(props.organizationId!)
          })
        ])
    }
  });

  return mutation;
}
