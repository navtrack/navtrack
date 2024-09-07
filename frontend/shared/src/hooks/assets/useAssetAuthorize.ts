import { useCallback } from "react";
import { useCurrentUserQuery } from "../queries/useCurrentUserQuery";
import { AssetRoleType } from "../../api/model/generated";

export function useAssetAuthorize() {
  const currentUser = useCurrentUserQuery();

  const assetAuthorize = useCallback(
    (assetId: string, role: AssetRoleType) => {
      const validRoles =
        role === AssetRoleType.Viewer
          ? [AssetRoleType.Owner, AssetRoleType.Viewer]
          : [role];

      return (
        currentUser.data?.assetRoles?.find(
          (x) => x.assetId === assetId && validRoles.includes(x.role)
        ) !== undefined
      );
    },
    [currentUser.data?.assetRoles]
  );

  return assetAuthorize;
}
