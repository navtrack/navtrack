import { AssetModel } from "../../api/model/generated";
import { useCurrentUser } from "../user/useCurrentUser";
import { AssetRoleType } from "../../api/model/custom/AssetRoleType";
import { useMemo } from "react";

export function useIsAssetOwner(asset?: AssetModel) {
  const currentUser = useCurrentUser();

  const isOwner = useMemo(() => {
    const index = asset?.users?.findIndex(
      (x) => x.userId === currentUser?.id && x.role === AssetRoleType.Owner
    );

    return index !== undefined && index >= 0;
  }, [asset?.users, currentUser?.id]);

  return isOwner;
}
