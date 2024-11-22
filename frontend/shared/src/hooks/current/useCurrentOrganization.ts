import { useMemo } from "react";
import { useOrganizationsQuery } from "../queries/organizations/useOrganizationsQuery";
import { useRecoilState } from "recoil";
import { currentOrganizationIdAtom } from "../../state/current";

export function useCurrentOrganization() {
  const [currentOrganizationId, setCurrentOrganizationId] = useRecoilState(
    currentOrganizationIdAtom
  );

  const organizations = useOrganizationsQuery();

  const organization = useMemo(
    () =>
      organizations.data?.items.find((org) => org.id === currentOrganizationId),
    [currentOrganizationId, organizations.data?.items]
  );

  return {
    id: currentOrganizationId,
    data: organization,
    isLoading: organizations.isLoading,
    set: setCurrentOrganizationId
  };
}
