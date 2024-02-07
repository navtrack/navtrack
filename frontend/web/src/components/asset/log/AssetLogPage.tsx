import { LocationFilter } from "../shared/location-filter/LocationFilter";
import { Map } from "../../ui/map/Map";
import { MapPin } from "../../ui/map/MapPin";
import { LogTable } from "./LogTable";
import { DEFAULT_MAP_CENTER } from "../../../constants";
import { useCurrentAsset } from "@navtrack/shared/hooks/assets/useCurrentAsset";
import { usePositionsQuery } from "@navtrack/shared/hooks/queries/usePositionsQuery";
import { useRef, useState, useMemo, useCallback, useEffect } from "react";
import { useRecoilValue } from "recoil";
import { locationFiltersSelector } from "../shared/location-filter/locationFilterState";
import { useLocationFilterKey } from "../shared/location-filter/useLocationFilterKey";
import { useKeyPress } from "@navtrack/shared/hooks/util/useKeyPress";
import { MapContainer } from "../../ui/map/MapContainer";
import { Card } from "../../ui/card/Card";

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
    <>
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
      <Card className="flex flex-grow">
        <MapContainer>
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
        </MapContainer>
      </Card>
    </>
  );
}
