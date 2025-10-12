import { getTeamsUsersListQueryKey, useTeamsUsersList } from "../../../api";

type UseTeamsQueryProps = {
  teamId?: string;
};

export const useTeamUsersQuery = (props: UseTeamsQueryProps) => {
  const query = useTeamsUsersList(props?.teamId!, {
    query: {
      queryKey: getTeamsUsersListQueryKey(`${props?.teamId}`),
      refetchOnWindowFocus: false,
      enabled: !!props?.teamId
    }
  });

  return query;
};
