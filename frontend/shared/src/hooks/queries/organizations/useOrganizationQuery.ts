import { useOrganizationsGet } from "../../../api/index-generated";

type UseOrganizationQueryProps = {
  organizationId?: string;
};

export function useOrganizationQuery(props: UseOrganizationQueryProps) {
  const query = useOrganizationsGet(props.organizationId!, {
    query: {
      enabled: props.organizationId !== undefined,
      refetchOnMount: false,
      refetchOnWindowFocus: false,
      refetchOnReconnect: false
    }
  });

  return query;
}
