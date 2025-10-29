import { faInfoCircle } from "@fortawesome/free-solid-svg-icons";
import { useContext, useEffect, useMemo, useState } from "react";
import { FormattedMessage } from "react-intl";
import { useAtomValue } from "jotai";
import { LocationFilter } from "../shared/location-filter/LocationFilter";
import { Card } from "../../ui/card/Card";
import { Icon } from "../../ui/icon/Icon";
import { Map } from "../../ui/map/Map";
import { MapTrip } from "../../ui/map/MapTrip";
import { Slider } from "../../ui/slider/Slider";
import { PositionCardItems } from "../shared/position-card/PositionCardItems";
import { useOnChange } from "@navtrack/shared/hooks/util/useOnChange";
import { CardMapWrapper } from "../../ui/map/CardMapWrapper";
import { TableV2 } from "../../ui/table/TableV2";
import { PositionDataModel, TripModel } from "@navtrack/shared/api/model";
import { useCurrentAsset } from "@navtrack/shared/hooks/current/useCurrentAsset";
import { useTripsQuery } from "@navtrack/shared/hooks/queries/assets/useTripsQuery";
import { locationFiltersSelector } from "../shared/location-filter/locationFilterState";
import { useLocationFilterKey } from "../shared/location-filter/useLocationFilterKey";
import { SlotContext } from "../../../app/SlotContext";
import { useShow } from "@navtrack/shared/hooks/util/useShow";
import { MapCenter } from "../../ui/map/MapCenter";
import { MapPin } from "../../ui/map/MapPin";

export function AssetTripsPage() {
  const show = useShow();
  const slots = useContext(SlotContext);

  const [selectedTrip, setSelectedTrip] = useState<TripModel>();
  const [selectedTripLocationIndex, setSelectedTripLocationIndex] =
    useState<number>(0);

  const selectedTripPosition = useMemo(
    () =>
      selectedTrip !== undefined && selectedTripLocationIndex !== undefined
        ? selectedTrip.positions[selectedTripLocationIndex]
        : undefined,
    [selectedTrip, selectedTripLocationIndex]
  );

  const [reverseGeocodePosition, setReverseGeocodePosition] = useState<
    PositionDataModel | undefined
  >(selectedTripPosition);

  const [showPin, setShowPin] = useState(false);

  const currentAsset = useCurrentAsset();
  const locationFilterKey = useLocationFilterKey("trips");
  const filters = useAtomValue(locationFiltersSelector(locationFilterKey));
  const query = useTripsQuery({ assetId: currentAsset.data?.id, ...filters });
  const hasTrips = (query.data?.items.length ?? 0) > 0;

  useOnChange(selectedTrip, () => {
    setShowPin(false);
    setSelectedTripLocationIndex(0);
  });

  useEffect(() => {
    if (selectedTripPosition && !reverseGeocodePosition) {
      setReverseGeocodePosition(selectedTripPosition);
    }
  }, [reverseGeocodePosition, selectedTripPosition]);

  return (
    <>
      <LocationFilter filterPage="trips" duration avgAltitude avgSpeed />
      <div>
        <TableV2<TripModel>
          columns={[
            {
              labelId: "generic.start-date",
              sort: "desc",
              sortValue: (row) => row.startPosition.date,
              row: (row) => show.dateTime(row.startPosition.date),
              sortable: true
            },
            {
              labelId: "generic.end-date",
              sortValue: (row) => row.endPosition.date,
              row: (row) => show.dateTime(row.endPosition.date),
              sortable: true,
              footerColSpan: 2
            },
            {
              labelId: "generic.duration",
              row: (row) => show.duration(row.duration),
              sortValue: (row) => row.duration,
              footerClassName: "font-semibold",
              footer: () =>
                hasTrips && show.duration(query.data?.totalDuration),
              sortable: true
            },
            {
              labelId: "generic.distance",
              row: (row) => show.distance(row.distance),
              sortValue: (row) => row.distance,
              footerClassName: "font-semibold",
              footer: () =>
                hasTrips && show.distance(query.data?.totalDistance),
              sortable: true
            },
            {
              labelId: "generic.max-speed",
              row: (row) => show.speed(row.maxSpeed),
              sortValue: (row) => row.maxSpeed,
              footerClassName: "font-semibold",
              footer: () => hasTrips && show.speed(query.data?.maxSpeed),
              sortable: true
            },
            // {
            //   labelId: "generic.avg-altitude",
            //   row: (row) => showAltitude(row.averageAltitude),
            //   sortValue: (row) => row.averageAltitude,
            //   footerClassName: "font-semibold",
            //   footer: () =>
            //     hasTrips && showAltitude(query.data?.totalAvgAltitude),
            //   sortable: true
            // },
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
          className="flex h-48 grow"
        />
      </div>
      <Card className="flex grow">
        <CardMapWrapper style={{ flexGrow: 2, minHeight: 250 }}>
          <Map>
            <MapTrip
              trip={selectedTrip}
              options={{
                initialZoom: 15,
                padding: { top: 20, left: 20, bottom: 10, right: 20 }
              }}
            />
            {selectedTripPosition && showPin && (
              <>
                <MapPin
                  pin={{
                    coordinates: selectedTripPosition.coordinates
                  }}
                  zIndexOffset={1}
                />
                <MapCenter position={selectedTripPosition.coordinates} />
              </>
            )}
          </Map>
        </CardMapWrapper>
      </Card>
      {selectedTrip !== undefined && (
        <>
          <Card>
            <div className="flex rounded-t-lg border-b border-gray-900/5 bg-gray-50 p-2 text-xs tracking-wider text-gray-500">
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
              <div className="flex grow bg-white px-3">
                <Slider
                  step={1}
                  min={0}
                  max={selectedTrip?.positions.length - 1}
                  value={selectedTripLocationIndex}
                  onChange={(value) => setSelectedTripLocationIndex(value)}
                  onMouseUp={() => {
                    if (selectedTripPosition) {
                      setReverseGeocodePosition(selectedTripPosition);
                    }
                  }}
                  onMouseDown={() => {
                    if (!showPin) {
                      setShowPin(true);
                    }
                  }}
                />
              </div>
            </div>
            {selectedTripPosition && (
              <div className="px-3 py-2">
                <div className="mb-2 flex justify-between">
                  <PositionCardItems position={selectedTripPosition} />
                </div>
                {reverseGeocodePosition !== undefined &&
                  slots?.assetLiveTrackingPositionCardExtraItems?.(
                    reverseGeocodePosition
                  )}
              </div>
            )}
          </Card>
        </>
      )}
    </>
  );
}
