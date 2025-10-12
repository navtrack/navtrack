import { getTeamsGetQueryKey, useTeamsGet } from "../../../api";

type UseTeamQueryProps = {
  teamId?: string;
};

export const useTeamQuery = (props: UseTeamQueryProps) => {
  const query = useTeamsGet(props?.teamId!, {
    query: {
      queryKey: getTeamsGetQueryKey(`${props?.teamId}`),
      refetchOnWindowFocus: false,
      enabled: !!props?.teamId
    }
  });

  return query;
};
