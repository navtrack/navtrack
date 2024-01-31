import { LocationFilter } from "../shared/location-filter/LocationFilter";
import { Map } from "../../ui/map/Map";
import { MapPin } from "../../ui/map/MapPin";
import { LogTable } from "./LogTable";
import { AuthenticatedLayoutTwoColumns } from "../../ui/layouts/authenticated/AuthenticatedLayoutTwoColumns";
import { DEFAULT_MAP_CENTER } from "../../../constants";
import { useCurrentAsset } from "@navtrack/shared/hooks/assets/useCurrentAsset";
import { usePositionsQuery } from "@navtrack/shared/hooks/queries/usePositionsQuery";
import { log } from "@navtrack/shared/utils/log";
import { useRef, useState, useMemo, useCallback, useEffect } from "react";
import { useRecoilValue } from "recoil";
import { locationFiltersSelector } from "../shared/location-filter/state";
import { useLocationFilterKey } from "../shared/location-filter/useLocationFilterKey";
import { useKeyPress } from "@navtrack/shared/hooks/util/useKeyPress";

export function AssetLogPage() {
  const currentAsset = useCurrentAsset();
  const locationFilterKey = useLocationFilterKey("log");
  const filters = useRecoilValue(locationFiltersSelector(locationFilterKey));
  const query = usePositionsQuery({
    assetId: currentAsset.data?.id,
    ...filters
  });
  const locationElements = useRef<Array<HTMLDivElement | null>>([]);
  const [selectedPositionIndex, setSelectedPositionIndex] = useState<
    number | undefined
  >();

  const position = useMemo(() => {
    if (selectedPositionIndex !== undefined) {
      return query.data?.items[selectedPositionIndex];
    }
  }, [query.data?.items, selectedPositionIndex]);

  useEffect(() => {
    if (selectedPositionIndex === undefined && query.data?.items) {
      setSelectedPositionIndex(0);
    }
  }, [query.data?.items, selectedPositionIndex, setSelectedPositionIndex]);

  const scrollToElement = useCallback((locationIndex: number) => {
    locationElements.current[locationIndex]?.scrollIntoView({
      behavior: "smooth",
      block: "center"
    });
  }, []);

  const setPreviousLocation = useCallback(() => {
    if (selectedPositionIndex !== undefined) {
      const newLocationIndex = selectedPositionIndex - 1;
      if (newLocationIndex >= 0) {
        setSelectedPositionIndex(newLocationIndex);
        scrollToElement(newLocationIndex);
      }
    }
  }, [scrollToElement, selectedPositionIndex, setSelectedPositionIndex]);

  const setNextLocation = useCallback(() => {
    if (selectedPositionIndex !== undefined) {
      const newLocationIndex = selectedPositionIndex + 1;
      if (query.data?.items && newLocationIndex < query.data?.items.length) {
        setSelectedPositionIndex(newLocationIndex);
        scrollToElement(newLocationIndex);
      }
    }
  }, [
    query.data?.items,
    scrollToElement,
    selectedPositionIndex,
    setSelectedPositionIndex
  ]);

  useKeyPress("ArrowDown", setNextLocation);
  useKeyPress("ArrowUp", setPreviousLocation);

  return (
    <AuthenticatedLayoutTwoColumns>
      <LocationFilter
        filterPage="log"
        center={
          position
            ? {
                latitude: position.latitude,
                longitude: position.longitude
              }
            : undefined
        }
      />
      <LogTable
        data={query.data}
        isLoading={query.isLoading}
        selectedPositionIndex={selectedPositionIndex}
        setSelectedPositionIndex={setSelectedPositionIndex}
        positionElements={locationElements}
      />
      <div className="flex flex-grow" style={{ flexBasis: 0 }}>
        <div className="flex flex-grow rounded-lg bg-white shadow">
          <Map
            center={
              position
                ? {
                    latitude: position.latitude,
                    longitude: position.longitude
                  }
                : DEFAULT_MAP_CENTER
            }
            initialZoom={16}>
            <MapPin position={position} follow />
          </Map>
        </div>
      </div>
    </AuthenticatedLayoutTwoColumns>
  );
}
