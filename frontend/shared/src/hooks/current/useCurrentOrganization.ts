import { useMemo } from "react";
import { useOrganizationsQuery } from "../queries/organizations/useOrganizationsQuery";
import { useRecoilValue } from "recoil";
import { currentOrganizationIdAtom } from "../../state/current";

export function useCurrentOrganization() {
  const currentOrganizationId = useRecoilValue(currentOrganizationIdAtom);
  const organizations = useOrganizationsQuery();

  const organization = useMemo(
    () =>
      organizations.data?.items.find((org) => org.id === currentOrganizationId),
    [currentOrganizationId, organizations.data?.items]
  );

  return {
    id: currentOrganizationId,
    data: organization,
    isLoading: organizations.isLoading
  };
}
