import { useCurrentAsset } from "@navtrack/ui-shared/newHooks/assets/useCurrentAsset";
import { useCallback, useEffect, useRef } from "react";
import { useRecoilState, useRecoilValue, useSetRecoilState } from "recoil";
import { locationFiltersSelector } from "../shared/location-filter/state";
import { useLocationFilterKey } from "../shared/location-filter/useLocationFilterKey";
import {
  selectedTripIndexAtom,
  selectedTripLocationIndexAtom,
  tripsAtom
} from "./state";
import { useTripsQuery } from "@navtrack/ui-shared/hooks/queries/useTripsQuery";

export default function useTrips() {
  const currentAsset = useCurrentAsset();
  const locationFilterKey = useLocationFilterKey("trips");
  const filters = useRecoilValue(locationFiltersSelector(locationFilterKey));
  const query = useTripsQuery({ assetId: currentAsset?.id, ...filters });
  const tripElements = useRef<Array<HTMLDivElement | null>>([]);
  const [selectedTripIndex, setSelectedTripIndex] = useRecoilState(
    selectedTripIndexAtom
  );
  const setSelectedTripLocationIndex = useSetRecoilState(
    selectedTripLocationIndexAtom
  );
  const setTrips = useSetRecoilState(tripsAtom);

  const setTripIndex = useCallback(
    (index: number) => {
      setSelectedTripIndex(index);
      setSelectedTripLocationIndex(1);
    },
    [setSelectedTripIndex, setSelectedTripLocationIndex]
  );

  useEffect(() => {
    if (query.data?.items) {
      setTrips(query.data?.items);
    }
    if (selectedTripIndex === undefined && query.data?.items) {
      setTripIndex(0);
    }
  }, [query.data?.items, selectedTripIndex, setTripIndex, setTrips]);

  const scrollToElement = useCallback((locationIndex: number) => {
    tripElements.current[locationIndex]?.scrollIntoView({
      behavior: "smooth",
      block: "center"
    });
  }, []);

  const setPreviousTrip = useCallback(() => {
    if (selectedTripIndex !== undefined) {
      const newLocationIndex = selectedTripIndex - 1;
      if (newLocationIndex >= 0) {
        setTripIndex(newLocationIndex);
        scrollToElement(newLocationIndex);
      }
    }
  }, [scrollToElement, selectedTripIndex, setTripIndex]);

  const setNextTrip = useCallback(() => {
    if (selectedTripIndex !== undefined) {
      const newLocationIndex = selectedTripIndex + 1;
      if (query.data?.items && newLocationIndex < query.data?.items.length) {
        setTripIndex(newLocationIndex);
        scrollToElement(newLocationIndex);
      }
    }
  }, [query.data?.items, scrollToElement, selectedTripIndex, setTripIndex]);

  const handleKeyPress = useCallback(
    (e: KeyboardEvent) => {
      if (e.code === "ArrowDown") {
        e.preventDefault();
        setNextTrip();
      } else if (e.code === "ArrowUp") {
        e.preventDefault();
        setPreviousTrip();
      }
    },
    [setNextTrip, setPreviousTrip]
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
    selectedTripIndex,
    setTripIndex,
    tripElements
  };
}
