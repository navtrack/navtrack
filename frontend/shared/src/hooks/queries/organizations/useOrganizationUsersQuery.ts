import { useOrganizationsUsersList } from "../../../api";

type UseUsersQueryProps = {
  organizationId?: string;
};

export const useOrganizationUsersQuery = (props: UseUsersQueryProps) => {
  const query = useOrganizationsUsersList(props.organizationId!, {
    query: {
      refetchOnWindowFocus: false,
      enabled: !!props.organizationId
    }
  });

  return query;
};
