import { useMemo, useState } from "react";
import { useAtomValue } from "jotai";
import { LocationFilter } from "../shared/location-filter/LocationFilter";
import { Card } from "../../ui/card/Card";
import { Map } from "../../ui/map/Map";
import { MapTrip } from "../../ui/map/MapTrip";
import { CardMapWrapper } from "../../ui/map/CardMapWrapper";
import { TableV2 } from "../../ui/table/TableV2";
import { PositionDataModel, TripModel } from "@navtrack/shared/api/model";
import { useCurrentAsset } from "@navtrack/shared/hooks/current/useCurrentAsset";
import { locationFiltersSelector } from "../shared/location-filter/locationFilterState";
import { useLocationFilterKey } from "../shared/location-filter/useLocationFilterKey";
import { useShow } from "@navtrack/shared/hooks/util/useShow";
import { GeocodeReverse } from "@navtrack/shared/components/components/geo/GeocodeReverse";
import { AssetTripDetailsPanel } from "./AssetTripDetailsPanel";
import { Button } from "../../ui/button/Button";
import { faMagnifyingGlass } from "@fortawesome/free-solid-svg-icons";
import { useAssetTripsQueries } from "@navtrack/shared/hooks/queries/assets/useAssetTripsQueries";
import { useOnChange } from "@navtrack/shared/hooks/util/useOnChange";
import { LoadingIndicator } from "@navtrack/shared/components/components/ui/loading-indicator/LoadingIndicator";

export function AssetTripsPage() {
  const show = useShow();
  const [selectedTrip, setSelectedTrip] = useState<TripModel>();
  const currentAsset = useCurrentAsset();
  const locationFilterKey = useLocationFilterKey("trips");
  const filters = useAtomValue(locationFiltersSelector(locationFilterKey));
  const query = useAssetTripsQueries({
    assetId: currentAsset.data?.id,
    ...filters
  });

  const [showTripPanel, setShowTripPanel] = useState(false);

  const trips = useMemo(
    () => query.queries.flatMap((q) => q.data?.items || []),
    [query]
  );
  const hasTrips = (trips.length ?? 0) > 0;
  useOnChange(query.totalDistance, () => {
    if (query.totalTrips === 0) {
      setSelectedTrip(undefined);
      setShowTripPanel(false);
    }
  });

  function LocationAndTimeCell(props: { position: PositionDataModel }) {
    return (
      <div>
        <div className="font-medium">{show.dateTime(props.position.date)}</div>
        <div className="text-xs">
          <GeocodeReverse coordinates={props.position.coordinates} />
        </div>
      </div>
    );
  }

  return (
    <>
      <LocationFilter filterPage="trips" duration altitude avgSpeed />
      {selectedTrip && (
        <AssetTripDetailsPanel
          trip={selectedTrip}
          open={showTripPanel}
          close={() => setShowTripPanel(false)}
        />
      )}
      <div>
        <TableV2<TripModel>
          columns={[
            {
              labelId: "generic.start",
              sort: "desc",
              sortValue: (row) => row.startPosition.date,
              rowClassName: "align-top w-1/2",
              row: (row) => (
                <LocationAndTimeCell position={row.startPosition} />
              ),
              headerClassName: "z-10",
              sortable: true,
              footerColSpan: 2,
              footer: () => (
                <div className="flex">
                  <span>{show.count("generic.trips.count", trips.length)}</span>
                  <LoadingIndicator
                    isLoading={query.isLoading}
                    className="ml-2"
                  />
                </div>
              ),
              footerClassName: "font-semibold"
            },
            {
              headerClassName: "z-10",
              labelId: "generic.end",
              sortValue: (row) => row.endPosition.date,
              rowClassName: "align-top w-1/2",
              row: (row) => <LocationAndTimeCell position={row.endPosition} />,
              sortable: true
            },
            {
              rowClassName: "w-24",
              labelId: "generic.duration",
              row: (row) => show.duration(row.duration),
              sortValue: (row) => row.duration,
              footerClassName: "font-semibold",
              footer: () => hasTrips && show.duration(query.totalDuration),
              sortable: true
            },
            {
              rowClassName: "w-24",
              labelId: "generic.distance",
              row: (row) => show.distance(row.distance),
              sortValue: (row) => row.distance,
              footerClassName: "font-semibold",
              footer: () => hasTrips && show.distance(query.totalDistance),
              sortable: true
            },
            {
              rowClassName: "w-24",
              labelId: "generic.average-speed",
              row: (row) => show.speed(row.averageSpeed),
              sortValue: (row) => row.averageSpeed,
              footerClassName: "font-semibold",
              footer: () => hasTrips && show.speed(query.averageSpeed),
              sortable: true
            },
            {
              rowClassName: "w-24",
              labelId: "generic.max-speed",
              row: (row) => show.speed(row.maxSpeed),
              sortValue: (row) => row.maxSpeed,
              footerClassName: "font-semibold",
              footer: () => hasTrips && show.speed(query.maxSpeed),
              sortable: true
            },
            {
              rowClassName: "w-32",
              labelId: "generic.average-fuel-consumption",
              row: (row) => show.fuelConsumption(row.averageFuelConsumption),
              sortValue: (row) => row.maxSpeed,
              footerClassName: "font-semibold",
              footer: () =>
                hasTrips && show.fuelConsumption(query.averageFuelConsumption),
              sortable: true
            },
            {
              rowClassName: "w-32",
              labelId: "generic.fuel-consumption",
              row: (row) => show.volume(row.fuelConsumption),
              sortValue: (row) => row.maxSpeed,
              footerClassName: "font-semibold",
              footer: () => hasTrips && show.volume(query.totalFuelConsumption),
              sortable: true
            },
            {
              row: () => (
                <Button
                  icon={faMagnifyingGlass}
                  color="white"
                  size="sm"
                  onClick={(e) => {
                    e.stopPropagation();
                    setShowTripPanel(true);
                  }}
                />
              ),
              footer: () => <></>
            }
          ]}
          rows={trips}
          setSelectedItem={(trip) => {
            setSelectedTrip(trip);
          }}
          isLoading={query.isLoading}
          className="flex h-80 "
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
          </Map>
        </CardMapWrapper>
      </Card>
    </>
  );
}
