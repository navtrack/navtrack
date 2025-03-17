import { useTeamsGetList } from "../../../api";

type UseTeamsQueryProps = {
  organizationId?: string;
};

export const useTeamsQuery = (props: UseTeamsQueryProps) => {
  const query = useTeamsGetList(props?.organizationId!, {
    query: {
      refetchOnWindowFocus: false,
      enabled: !!props?.organizationId
    }
  });

  return query;
};
