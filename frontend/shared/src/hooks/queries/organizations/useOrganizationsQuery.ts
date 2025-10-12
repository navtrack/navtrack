import {
  getOrganizationsListQueryKey,
  useOrganizationsList
} from "../../../api";

export function useOrganizationsQuery() {
  const query = useOrganizationsList({
    query: {
      queryKey: getOrganizationsListQueryKey(),
      refetchOnMount: false,
      refetchOnWindowFocus: false,
      refetchOnReconnect: false
    }
  });

  return query;
}
