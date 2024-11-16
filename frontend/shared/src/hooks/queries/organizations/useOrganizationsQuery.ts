import { useOrganizationsList } from "../../../api/index-generated";

export function useOrganizationsQuery() {
  const query = useOrganizationsList({
    query: {
      refetchOnMount: false,
      refetchOnWindowFocus: false,
      refetchOnReconnect: false
    }
  });

  return query;
}
