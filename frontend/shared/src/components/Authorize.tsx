import { ReactNode, useMemo } from "react";
import { useCurrentAsset } from "../hooks/assets/useCurrentAsset";
import { useAssetAuthorize } from "../hooks/assets/useAssetAuthorize";
import { AssetRoleType } from "../api/model/generated";

type AuthorizeProps = {
  role: AssetRoleType;
  children: ReactNode;
};

export function Authorize(props: AuthorizeProps) {
  const currentAsset = useCurrentAsset();
  const assetAuthorize = useAssetAuthorize();

  const isAuthorized = useMemo(
    () =>
      currentAsset.data && assetAuthorize(currentAsset.data?.id, props.role),
    [assetAuthorize, currentAsset.data, props.role]
  );

  return <>{isAuthorized ? props.children : null}</>;
}
