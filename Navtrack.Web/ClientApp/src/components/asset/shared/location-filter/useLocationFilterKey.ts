import { useCurrentAsset } from "@navtrack/navtrack-app-shared";
import { useMemo } from "react";

export default function useDefaultFilterKey(page: string) {
  const currentAsset = useCurrentAsset();

  const key = useMemo(() => `${page}:${currentAsset}`, [currentAsset, page]);

  return key;
}
