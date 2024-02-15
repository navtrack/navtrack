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
            value: (row) => row.startPosition.dateTime,
            render: (row) => showDateTime(row.startPosition.dateTime),
            sortable: true
          },
          {
            labelId: "generic.end-date",
            value: (row) => row.endPosition.dateTime,
            render: (row) => showDateTime(row.endPosition.dateTime),
            sortable: true
          },
          {
            labelId: "generic.duration",
            render: (row) => showDuration(row.duration),
            value: (row) => row.duration,
            sortable: true
          },
          {
            labelId: "generic.distance",
            render: (row) => showDistance(row.distance),
            value: (row) => row.distance,
            sortable: true
          },
          {
            labelId: "generic.avg-speed",
            render: (row) => showSpeed(row.averageSpeed),
            value: (row) => row.averageSpeed,
            sortable: true
          },
          {
            labelId: "generic.avg-altitude",
            render: (row) => showAltitude(row.averageAltitude),
            value: (row) => row.averageAltitude,
            sortable: true
          },
          {
            labelId: "generic.positions",
            render: (row) => row.positions.length,
            value: (row) => row.positions.length,
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
