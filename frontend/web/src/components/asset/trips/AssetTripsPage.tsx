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
import { useTable } from "../../ui/table/useTable";

export function AssetTripsPage() {
  const show = useShow();
  const [selectedTripIndex, setSelectedTripIndex] = useState(0);
  const currentAsset = useCurrentAsset();
  const locationFilterKey = useLocationFilterKey("trips");
  const filters = useAtomValue(locationFiltersSelector(locationFilterKey));
  const query = useAssetTripsQueries({
    assetId: currentAsset.data?.id,
    ...filters
  });

  const [showTripPanel, setShowTripPanel] = useState(false);

  const hasTrips = (query.allTrips?.length ?? 0) > 0;

  useOnChange(query.totalDistance, () => {
    if (query.totalTrips === 0) {
      setSelectedTripIndex(0);
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

  const selectNextTrip = () => {
    setSelectedTripIndex((prevIndex) =>
      prevIndex < (query.allTrips?.length ?? 0) - 1 ? prevIndex + 1 : prevIndex
    );
  };

  const selectPreviousTrip = () => {
    setSelectedTripIndex((prevIndex) =>
      prevIndex > 0 ? prevIndex - 1 : prevIndex
    );
  };

  const table = useTable<TripModel>({
    rows: query.allTrips,
    selection: true,
    columns: [
      {
        labelId: "generic.start",
        sort: "desc",
        value: (row) => row.startPosition.date,
        rowClassName: "align-top w-1/2",
        row: (row) => <LocationAndTimeCell position={row.startPosition} />,
        headerClassName: "z-10",
        footer: () => (
          <div className="flex">
            <span>
              {show.count("generic.trips.count", query.allTrips?.length ?? 0)}
            </span>
            <LoadingIndicator isLoading={query.isLoading} className="ml-2" />
          </div>
        ),
        footerClassName: "font-semibold"
      },
      {
        headerClassName: "z-10",
        labelId: "generic.end",
        value: (row) => row.endPosition.date,
        rowClassName: "align-top w-1/2",
        row: (row) => <LocationAndTimeCell position={row.endPosition} />
      },
      {
        rowClassName: "w-24",
        labelId: "generic.duration",
        row: (row) => show.duration(row.duration),
        value: (row) => row.duration,
        footerClassName: "font-semibold",
        footer: () => hasTrips && show.duration(query.totalDuration)
      },
      {
        rowClassName: "w-24",
        labelId: "generic.distance",
        row: (row) => show.distance(row.distance),
        value: (row) => row.distance,
        footerClassName: "font-semibold",
        footer: () => hasTrips && show.distance(query.totalDistance)
      },
      {
        rowClassName: "w-24",
        labelId: "generic.average-speed",
        row: (row) => show.speed(row.averageSpeed),
        value: (row) => row.averageSpeed,
        footerClassName: "font-semibold",
        footer: () => hasTrips && show.speed(query.averageSpeed)
      },
      {
        rowClassName: "w-24",
        labelId: "generic.max-speed",
        row: (row) => show.speed(row.maxSpeed),
        value: (row) => row.maxSpeed,
        footerClassName: "font-semibold",
        footer: () => hasTrips && show.speed(query.maxSpeed)
      },
      {
        rowClassName: "w-32",
        labelId: "generic.average-fuel-consumption",
        row: (row) => show.fuelConsumption(row.averageFuelConsumption),
        value: (row) => row.maxSpeed,
        footerClassName: "font-semibold",
        footer: () =>
          hasTrips && show.fuelConsumption(query.averageFuelConsumption)
      },
      {
        rowClassName: "w-32",
        labelId: "generic.fuel-consumption",
        row: (row) => show.volume(row.fuelConsumption),
        value: (row) => row.maxSpeed,
        footerClassName: "font-semibold",
        footer: () => hasTrips && show.volume(query.totalFuelConsumption)
      },
      {
        row: (_, index) => (
          <Button
            icon={faMagnifyingGlass}
            color="white"
            size="sm"
            onClick={(e) => {
              e.stopPropagation();
              setSelectedTripIndex(index);
              setShowTripPanel(true);
            }}
          />
        )
      }
    ]
  });

  const selectedTripForModal = useMemo(() => {
    return table.props.rows?.[selectedTripIndex];
  }, [selectedTripIndex, table.props.rows]);

  return (
    <>
      <LocationFilter filterPage="trips" duration altitude avgSpeed />
      {selectedTripForModal && (
        <AssetTripDetailsPanel
          trip={selectedTripForModal}
          open={showTripPanel}
          close={() => setShowTripPanel(false)}
          previous={selectPreviousTrip}
          next={selectNextTrip}
          previousDisabled={selectedTripIndex === 0}
          nextDisabled={selectedTripIndex === (query.allTrips?.length ?? 0) - 1}
        />
      )}
      <TableV2<TripModel> className="flex h-80" {...table.props} />
      <Card className="flex grow">
        <CardMapWrapper style={{ flexGrow: 2, minHeight: 250 }}>
          <Map>
            <MapTrip
              trip={table.selectedItem}
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
