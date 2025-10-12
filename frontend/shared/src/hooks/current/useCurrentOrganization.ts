import { useEffect, useMemo } from "react";
import { useOrganizationsQuery } from "../queries/organizations/useOrganizationsQuery";
import { useAtom } from "jotai";
import {
  currentOrganizationIdAtom,
  currentOrganizationIdInitializedAtom
} from "../../state/current";

export function useCurrentOrganization() {
  const [currentOrganizationId, setCurrentOrganizationId] = useAtom(
    currentOrganizationIdAtom
  );
  const [
    currentOrganizationIdInitialized,
    setCurrentOrganizationIdInitialized
  ] = useAtom(currentOrganizationIdInitializedAtom);

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
    set: setCurrentOrganizationId
  };
}
