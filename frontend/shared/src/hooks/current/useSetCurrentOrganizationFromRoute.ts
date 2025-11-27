import { useEffect } from "react";
import { useMatch } from "react-router-dom";
import { useCurrentAsset } from "./useCurrentAsset";
import { useAtom } from "jotai";
import { currentOrganizationIdAtom } from "../../state/current";
import { useCurrentTeam } from "./useCurrentTeam";

export function useSetCurrentOrganizationFromRoute() {
  const [currentOrganizationId, setCurrentOrganizationId] = useAtom(
    currentOrganizationIdAtom
  );

  const orgMatch = useMatch("/organizations/:id/*");

  const assetMatch = useMatch("/assets/:id/*");
  const currentAsset = useCurrentAsset();

  const teamMatch = useMatch("/teams/:id/*");
  const currentTeam = useCurrentTeam();

  useEffect(() => {
    if (
      orgMatch?.params.id !== undefined &&
      orgMatch.params.id !== currentOrganizationId
    ) {
      setCurrentOrganizationId(orgMatch.params.id);
    }
  }, [currentOrganizationId, orgMatch?.params.id, setCurrentOrganizationId]);

  useEffect(() => {
    if (
      assetMatch?.params.id !== undefined &&
      currentAsset.data?.organizationId !== undefined &&
      currentAsset.data?.organizationId !== currentOrganizationId
    ) {
      setCurrentOrganizationId(currentAsset.data?.organizationId);
    }
  }, [
    assetMatch?.params.id,
    currentAsset.data?.organizationId,
    currentOrganizationId,
    setCurrentOrganizationId
  ]);

  useEffect(() => {
    if (
      teamMatch?.params.id !== undefined &&
      currentTeam.data?.organizationId !== undefined &&
      currentTeam.data?.organizationId !== currentOrganizationId
    ) {
      setCurrentOrganizationId(currentTeam.data?.organizationId);
    }
  }, [
    currentOrganizationId,
    currentTeam.data?.organizationId,
    setCurrentOrganizationId,
    teamMatch?.params.id
  ]);

  useEffect(() => {
    if (
      orgMatch?.params.id === undefined &&
      assetMatch?.params.id === undefined &&
      teamMatch?.params.id === undefined &&
      currentOrganizationId !== undefined
    ) {
      setCurrentOrganizationId(undefined);
    }
  }, [
    assetMatch?.params.id,
    currentOrganizationId,
    orgMatch?.params.id,
    setCurrentOrganizationId,
    teamMatch?.params.id
  ]);
}
