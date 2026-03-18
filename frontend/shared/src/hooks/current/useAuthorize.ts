import { useCallback } from "react";
import { useCurrentUserQuery } from "../queries/user/useCurrentUserQuery";
import {
  AssetUserRole,
  OrganizationUserRole,
  TeamUserRole
} from "../../api/model";
import { useCurrentOrganization } from "./useCurrentOrganization";
import { useCurrentAsset } from "./useCurrentAsset";

export function useAuthorize() {
  const currentUser = useCurrentUserQuery();
  const currentOrganization = useCurrentOrganization();
  const currentAsset = useCurrentAsset();

  const authorizeOrganization = useCallback(
    (userRole: OrganizationUserRole) => {
      const organization = currentUser.data?.organizations?.find(
        (x) => x.organizationId === currentOrganization.id
      );

      switch (userRole) {
        case OrganizationUserRole.Owner:
          return organization?.userRole === OrganizationUserRole.Owner;
        case OrganizationUserRole.Member:
          return (
            organization?.userRole === OrganizationUserRole.Owner ||
            organization?.userRole === OrganizationUserRole.Member
          );
        default:
          return false;
      }
    },
    [currentOrganization.id, currentUser.data?.organizations]
  );

  const assetAuthorize = useCallback(
    (userRole: AssetUserRole) => {
      const isOrganizationOwner = authorizeOrganization(
        OrganizationUserRole.Owner
      );

      if (isOrganizationOwner) {
        return true;
      }

      const asset = currentUser.data?.assets?.find(
        (x) => x.assetId === currentAsset.id
      );

      switch (userRole) {
        case AssetUserRole.Owner:
          return asset?.userRole === AssetUserRole.Owner;
        case AssetUserRole.Viewer:
          return (
            asset?.userRole === AssetUserRole.Owner ||
            asset?.userRole === AssetUserRole.Viewer
          );
        default:
          return false;
      }
    },
    [authorizeOrganization, currentAsset.id, currentUser.data?.assets]
  );

  const authorizeTeam = useCallback(
    (userRole: TeamUserRole) => {
      const isOrganizationOwner = authorizeOrganization(
        OrganizationUserRole.Owner
      );

      if (isOrganizationOwner) {
        return true;
      }

      const team = currentUser.data?.teams?.find(
        (x) => x.teamId === currentOrganization.id
      );

      switch (userRole) {
        case TeamUserRole.Owner:
          return team?.userRole === TeamUserRole.Owner;
        case TeamUserRole.Member:
          return (
            team?.userRole === TeamUserRole.Owner ||
            team?.userRole === TeamUserRole.Member
          );
        default:
          return false;
      }
    },
    [currentOrganization.id, currentUser.data?.teams]
  );

  return {
    organization: authorizeOrganization,
    asset: assetAuthorize,
    team: authorizeTeam
  };
}
