import { getTeamsGetListQueryKey, useTeamsGetList } from "../../../api";

type UseTeamsQueryProps = {
  organizationId?: string;
};

export const useTeamsQuery = (props: UseTeamsQueryProps) => {
  const query = useTeamsGetList(props?.organizationId!, {
    query: {
      queryKey: getTeamsGetListQueryKey(`${props?.organizationId}`),
      refetchOnWindowFocus: false,
      enabled: !!props?.organizationId
    }
  });

  return query;
};
