import { useCurrentAsset } from "@navtrack/shared/hooks/current/useCurrentAsset";
import { useAtomValue } from "jotai";
import { useMemo } from "react";
import { locationFiltersSelector } from "./locationFilterState";
import {
  LocationFilterConfiguration,
  LocationFilterPage,
  LocationFilterType
} from "./locationFilterTypes";

type LocationFilterProps = {
  page: LocationFilterPage;
  filters?: LocationFilterType[];
};

export function useLocationFilter(props: LocationFilterProps) {
  const currentAsset = useCurrentAsset();

  const key = useMemo(
    () => `${props.page}:${currentAsset.data?.id}`,
    [currentAsset, props.page]
  );

  const filters = useAtomValue(locationFiltersSelector(key));

  const configuration: LocationFilterConfiguration = useMemo(
    () => ({ filterKey: key, filters: props.filters ?? [] }),
    [key, props.filters]
  );

  return { filters, configuration };
}
