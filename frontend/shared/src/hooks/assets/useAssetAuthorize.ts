import { useCallback } from "react";
import { useCurrentUserQuery } from "../queries/useCurrentUserQuery";
import { useAssetsQuery } from "../queries/useAssetsQuery";
import { AssetRoleType } from "../../api/model/generated";

export function useAssetAuthorize() {
  const assets = useAssetsQuery();
  const currentUser = useCurrentUserQuery();

  const assetAuthorize = useCallback(
    (assetId: string, role: AssetRoleType) => {
      const validRoles =
        role === AssetRoleType.Viewer
          ? [AssetRoleType.Owner, AssetRoleType.Viewer]
          : [role];

      return assets.data?.items
        .find((x) => x.id === assetId)
        ?.userRoles?.find(
          (x) =>
            x.userId === currentUser.data?.id && validRoles.includes(x.role)
        );
    },
    [assets.data?.items, currentUser.data?.id]
  );

  return assetAuthorize;
}
