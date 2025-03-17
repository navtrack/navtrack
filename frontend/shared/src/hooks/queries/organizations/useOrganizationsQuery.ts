import { useOrganizationsList } from "../../../api";

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
