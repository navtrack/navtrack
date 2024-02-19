import { faInfoCircle } from "@fortawesome/free-solid-svg-icons";
import { useState } from "react";
import { FormattedMessage } from "react-intl";
import { useRecoilState, useRecoilValue } from "recoil";
import { LocationFilter } from "../shared/location-filter/LocationFilter";
import { Card } from "../../ui/card/Card";
import { Icon } from "../../ui/icon/Icon";
import { Map } from "../../ui/map/Map";
import { MapPin } from "../../ui/map/MapPin";
import { MapTrip } from "../../ui/map/MapTrip";
import { Slider } from "../../ui/slider/Slider";
import { PositionBar } from "../shared/position-bar/PositionBar";
import {
  selectedTripAtom,
  selectedTripPositionIndexAtom,
  selectedTripPositionSelector
} from "./tripsState";
import { useOnChange } from "@navtrack/shared/hooks/util/useOnChange";
import { MapContainer } from "../../ui/map/MapContainer";
import { TableV2 } from "../../ui/table/TableV2";
import { TripModel } from "@navtrack/shared/api/model/generated";
import { useDateTime } from "@navtrack/shared/hooks/util/useDateTime";
import { useDistance } from "@navtrack/shared/hooks/util/useDistance";
import { useCurrentAsset } from "@navtrack/shared/hooks/assets/useCurrentAsset";
import { useTripsQuery } from "@navtrack/shared/hooks/queries/useTripsQuery";
import { locationFiltersSelector } from "../shared/location-filter/locationFilterState";
import { useLocationFilterKey } from "../shared/location-filter/useLocationFilterKey";
import { DEFAULT_MAP_CENTER } from "../../../constants";

export function AssetTripsPage() {
  const [selectedTrip, setSelectedTrip] = useRecoilState(selectedTripAtom);
  const selectedTripPosition = useRecoilValue(selectedTripPositionSelector);
  const [selectedTripLocationIndex, setSelectedTripLocationIndex] =
    useRecoilState(selectedTripPositionIndexAtom);
  const [showPin, setShowPin] = useState(false);

  const { showDuration, showDateTime } = useDateTime();
  const { showSpeed, showDistance, showAltitude } = useDistance();

  const currentAsset = useCurrentAsset();
  const locationFilterKey = useLocationFilterKey("trips");
  const filters = useRecoilValue(locationFiltersSelector(locationFilterKey));
  const query = useTripsQuery({ assetId: currentAsset.data?.id, ...filters });
  const hasTrips = (query.data?.items.length ?? 0) > 0;

  useOnChange(selectedTrip, () => {
    setShowPin(false);
    setSelectedTripLocationIndex(1);
  });

  return (
    <>
      <LocationFilter filterPage="trips" duration avgAltitude avgSpeed />
      <TableV2<TripModel>
        columns={[
          {
            labelId: "generic.start-date",
            sort: "desc",
            sortValue: (row) => row.startPosition.dateTime,
            row: (row) => showDateTime(row.startPosition.dateTime),
            sortable: true
          },
          {
            labelId: "generic.end-date",
            sortValue: (row) => row.endPosition.dateTime,
            row: (row) => showDateTime(row.endPosition.dateTime),
            sortable: true,
            footerColSpan: 2
          },
          {
            labelId: "generic.duration",
            row: (row) => showDuration(row.duration),
            sortValue: (row) => row.duration,
            footerClassName: "font-semibold",
            footer: () => hasTrips && showDuration(query.data?.totalDuration),
            sortable: true
          },
          {
            labelId: "generic.distance",
            row: (row) => showDistance(row.distance),
            sortValue: (row) => row.distance,
            footerClassName: "font-semibold",
            footer: () => hasTrips && showDistance(query.data?.totalDistance),
            sortable: true
          },
          {
            labelId: "generic.avg-speed",
            row: (row) => showSpeed(row.averageSpeed),
            sortValue: (row) => row.averageSpeed,
            footerClassName: "font-semibold",
            footer: () => hasTrips && showSpeed(query.data?.totalAvgSpeed),
            sortable: true
          },
          {
            labelId: "generic.avg-altitude",
            row: (row) => showAltitude(row.averageAltitude),
            sortValue: (row) => row.averageAltitude,
            footerClassName: "font-semibold",
            footer: () =>
              hasTrips && showAltitude(query.data?.totalAvgAltitude),
            sortable: true
          },
          {
            labelId: "generic.positions",
            row: (row) => row.positions.length,
            sortValue: (row) => row.positions.length,
            footerClassName: "font-semibold",
            footer: () => hasTrips && query.data?.totalPositions,
            sortable: true
          }
        ]}
        rows={query.data?.items}
        setSelectedItem={setSelectedTrip}
        className="flex h-44 flex-grow"
      />
      {selectedTrip !== undefined && (
        <>
          <Card>
            <div className="flex rounded-t-lg border-b bg-gray-50 p-2 text-xs tracking-wider text-gray-500">
              <div className="font-medium uppercase">
                <FormattedMessage id="generic.position" />
              </div>
              <div className="w-full text-right tracking-normal">
                <Icon icon={faInfoCircle} />
                <span className="ml-1">
                  <FormattedMessage id="assets.trips.position.tip" />
                </span>
              </div>
            </div>
            <div className="flex" style={{ flexBasis: 0 }}>
              <div className="flex flex-grow bg-white px-3">
                <Slider
                  step={1}
                  min={1}
                  max={selectedTrip?.positions.length}
                  value={selectedTripLocationIndex}
                  onChange={(value) => setSelectedTripLocationIndex(value)}
                  onMouseDown={() => {
                    if (!showPin) {
                      setShowPin(true);
                    }
                  }}
                />
              </div>
            </div>
            {selectedTripPosition && (
              <div className="p-2">
                <PositionBar position={selectedTripPosition} />
              </div>
            )}
          </Card>
        </>
      )}
      <Card className="flex flex-grow">
        <MapContainer style={{ flexGrow: 2, minHeight: "200px" }}>
          <Map center={DEFAULT_MAP_CENTER} initialZoom={16}>
            <MapTrip trip={selectedTrip} />
            {selectedTripPosition && showPin && (
              <MapPin position={{ ...selectedTripPosition }} zIndexOffset={1} />
            )}
          </Map>
        </MapContainer>
      </Card>
    </>
  );
}
