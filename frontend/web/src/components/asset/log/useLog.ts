import { useCurrentAsset } from "@navtrack/ui-shared/newHooks/assets/useCurrentAsset";
import { useCallback, useEffect, useMemo, useRef, useState } from "react";
import { useRecoilValue } from "recoil";
import { locationFiltersSelector } from "../shared/location-filter/state";
import { useLocationFilterKey } from "../shared/location-filter/useLocationFilterKey";
import { useLocationsQuery } from "@navtrack/ui-shared/hooks/queries/useLocationsQuery";

export default function useLog() {
  const currentAsset = useCurrentAsset();
  const locationFilterKey = useLocationFilterKey("log");
  const filters = useRecoilValue(locationFiltersSelector(locationFilterKey));
  const query = useLocationsQuery({ assetId: currentAsset?.id, ...filters });
  const locationElements = useRef<Array<HTMLDivElement | null>>([]);
  const [selectedLocationIndex, setSelectedLocationIndex] = useState<
    number | undefined
  >();

  const location = useMemo(() => {
    if (selectedLocationIndex !== undefined) {
      return query.data?.items[selectedLocationIndex];
    }
  }, [query.data?.items, selectedLocationIndex]);

  const setLocationIndex = useCallback(
    (index: number) => {
      let location = query.data?.items[index];
      if (location) {
        setSelectedLocationIndex(index);
      }
    },
    [query.data?.items]
  );

  useEffect(() => {
    if (selectedLocationIndex === undefined && query.data?.items) {
      setLocationIndex(0);
    }
  }, [query.data?.items, selectedLocationIndex, setLocationIndex]);

  const scrollToElement = useCallback((locationIndex: number) => {
    locationElements.current[locationIndex]?.scrollIntoView({
      behavior: "smooth",
      block: "center"
    });
  }, []);

  const setPreviousLocation = useCallback(() => {
    if (selectedLocationIndex !== undefined) {
      const newLocationIndex = selectedLocationIndex - 1;
      if (newLocationIndex >= 0) {
        setLocationIndex(newLocationIndex);
        scrollToElement(newLocationIndex);
      }
    }
  }, [scrollToElement, selectedLocationIndex, setLocationIndex]);

  const setNextLocation = useCallback(() => {
    if (selectedLocationIndex !== undefined) {
      const newLocationIndex = selectedLocationIndex + 1;
      if (query.data?.items && newLocationIndex < query.data?.items.length) {
        setLocationIndex(newLocationIndex);
        scrollToElement(newLocationIndex);
      }
    }
  }, [
    query.data?.items,
    scrollToElement,
    selectedLocationIndex,
    setLocationIndex
  ]);

  const handleKeyPress = useCallback(
    (e: KeyboardEvent) => {
      if (e.code === "ArrowDown") {
        e.preventDefault();
        setNextLocation();
      } else if (e.code === "ArrowUp") {
        e.preventDefault();
        setPreviousLocation();
      }
    },
    [setNextLocation, setPreviousLocation]
  );

  useEffect(() => {
    window.addEventListener("keydown", handleKeyPress);
    return () => {
      window.removeEventListener("keydown", handleKeyPress);
    };
  }, [handleKeyPress]);

  return {
    data: query.data,
    isLoading: query.isLoading,
    selectedLocationIndex,
    setSelectedLocationIndex: setLocationIndex,
    locationElements,
    location
  };
}
