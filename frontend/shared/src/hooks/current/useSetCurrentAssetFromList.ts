import { useEffect } from "react";
import { useRecoilState } from "recoil";
import { currentAssetIdAtom } from "../../state/current";
import { useCurrentOrganization } from "./useCurrentOrganization";
import { useAssetsQuery } from "../queries/assets/useAssetsQuery";

export function useSetCurrentAssetFromList() {
  const [currentAssetId, setCurrentAssetId] =
    useRecoilState(currentAssetIdAtom);
  const currentOrganization = useCurrentOrganization();
  const assets = useAssetsQuery({ organizationId: currentOrganization.id });

  useEffect(() => {
    if (!currentAssetId && assets.data && assets.data?.items.length > 0) {
      setCurrentAssetId(assets.data.items[0].id);
    }
  }, [assets, currentAssetId, setCurrentAssetId]);
}
