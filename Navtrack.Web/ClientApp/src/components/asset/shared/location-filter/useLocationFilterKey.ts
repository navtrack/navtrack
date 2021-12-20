import { useMemo } from "react";
import useCurrentAssetId from "../../../../hooks/assets/useCurrentAssetId";

export default function useDefaultFilterKey(page: string) {
  const currentAssetId = useCurrentAssetId();

  const key = useMemo(
    () => `${page}:${currentAssetId}`,
    [currentAssetId, page]
  );

  return key;
}
