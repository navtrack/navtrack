import { useUsersList } from "../../../api/index-generated";

type UseUsersQueryProps = {
  organizationId?: string;
};

export const useOrganizationUsersQuery = (props: UseUsersQueryProps) => {
  const query = useUsersList(props.organizationId!, {
    query: {
      refetchOnWindowFocus: false,
      enabled: !!props.organizationId
    }
  });

  return query;
};
