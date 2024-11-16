import { useCallback } from "react";
import { useCurrentUserQuery } from "../queries/user/useCurrentUserQuery";
import { AssetUserRole } from "../../api/model/generated";

export function useAssetAuthorize() {
  const currentUser = useCurrentUserQuery();

  const assetAuthorize = useCallback(
    (assetId: string, role: AssetUserRole) => {
      const validRoles =
        role === AssetUserRole.Viewer
          ? [AssetUserRole.Owner, AssetUserRole.Viewer]
          : [role];

      return (
        currentUser.data?.assets?.find(
          (x) => x.assetId === assetId && validRoles.includes(x.userRole)
        ) !== undefined
      );
    },
    [currentUser.data?.assets]
  );

  return assetAuthorize;
}
