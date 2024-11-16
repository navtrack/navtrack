import { useCurrentAsset } from "@navtrack/shared/hooks/current/useCurrentAsset";
import { useMemo } from "react";

export type LocationFilterPage = "trips" | "log";

export const useLocationFilterKey = (page: LocationFilterPage) => {
  const currentAsset = useCurrentAsset();

  const key = useMemo(
    () => `${page}:${currentAsset.data?.id}`,
    [currentAsset, page]
  );

  return key;
};
