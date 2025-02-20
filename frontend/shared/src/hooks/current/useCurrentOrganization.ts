import { useEffect, useMemo } from "react";
import { useOrganizationsQuery } from "../queries/organizations/useOrganizationsQuery";
import { useRecoilState } from "recoil";
import {
  currentOrganizationIdAtom,
  currentOrganizationIdInitializedAtom
} from "../../state/current";

export function useCurrentOrganization() {
  const [currentOrganizationId, setCurrentOrganizationId] = useRecoilState(
    currentOrganizationIdAtom
  );
  const [
    currentOrganizationIdInitialized,
    setCurrentOrganizationIdInitialized
  ] = useRecoilState(currentOrganizationIdInitializedAtom);

  const organizations = useOrganizationsQuery();

  const organization = useMemo(
    () =>
      organizations.data?.items.find((org) => org.id === currentOrganizationId),
    [currentOrganizationId, organizations.data?.items]
  );

  useEffect(() => {
    if (currentOrganizationId !== null && !currentOrganizationIdInitialized) {
      setCurrentOrganizationIdInitialized(true);
    }
  }, [
    currentOrganizationId,
    currentOrganizationIdInitialized,
    setCurrentOrganizationIdInitialized
  ]);

  return {
    id: currentOrganizationId,
    data: organization,
    initialized: currentOrganizationIdInitialized,
    isLoading: organizations.isLoading,
    set: setCurrentOrganizationId,
    reset: () => {
      setCurrentOrganizationId(undefined);
    }
  };
}
