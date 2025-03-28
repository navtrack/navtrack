import { useTeamsGet } from "../../../api";

type UseTeamQueryProps = {
  teamId?: string;
};

export const useTeamQuery = (props: UseTeamQueryProps) => {
  const query = useTeamsGet(props?.teamId!, {
    query: {
      refetchOnWindowFocus: false,
      enabled: !!props?.teamId
    }
  });

  return query;
};
