import { useContext, useMemo } from "react";
import { useOrganizationsQuery } from "../queries/organizations/useOrganizationsQuery";
import { CurrentContext } from "./CurrentContextProvider";

export function useCurrentOrganization() {
  const current = useContext(CurrentContext);
  const organizations = useOrganizationsQuery();

  const organization = useMemo(
    () =>
      (organizations.data?.items ?? []).find(
        (org) => org.id === current.organizationId
      ),
    [current.organizationId, organizations.data?.items]
  );

  return {
    id: organization?.id,
    data: organization,
    isLoading: organizations.isLoading
  };
}
