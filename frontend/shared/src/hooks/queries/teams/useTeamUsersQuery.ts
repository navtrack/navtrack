import { useTeamsUsersList } from "../../../api/index-generated";

type UseTeamsQueryProps = {
  teamId?: string;
};

export const useTeamUsersQuery = (props: UseTeamsQueryProps) => {
  const query = useTeamsUsersList(props?.teamId!, {
    query: {
      refetchOnWindowFocus: false,
      enabled: !!props?.teamId
    }
  });

  return query;
};
