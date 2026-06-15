import { LocationFilter } from "../shared/location-filter/LocationFilter";
import { useCurrentAsset } from "@navtrack/shared/hooks/current/useCurrentAsset";
import { TableV2 } from "../../ui/table/TableV2";
import { useShow } from "@navtrack/shared/hooks/util/useShow";
import { GeocodeReverse } from "@navtrack/shared/components/components/geo/GeocodeReverse";
import {
  TripStopModel,
  useAssetTripsStopsQueries
} from "@navtrack/shared/hooks/queries/assets/useAssetTripsStopsQueries";
import { Button } from "../../ui/button/Button";
import { faMagnifyingGlass } from "@fortawesome/free-solid-svg-icons";
import { useMemo, useState } from "react";
import { AssetTripStopDetailsPanel } from "./AssetTripStopDetailsPanel";
import { useTable } from "../../ui/table/useTable";
import { useLocationFilter } from "../shared/location-filter/useLocationFilter";

export function AssetReportsTripStopsPage() {
  const show = useShow();
  const currentAsset = useCurrentAsset();
  const filter = useLocationFilter({
    page: "asset-reports-stops"
  });
  const tripsStopsReport = useAssetTripsStopsQueries({
    assetId: currentAsset.data?.id,
    startDate: filter.filters.startDate,
    endDate: filter.filters.endDate
  });
  const [showTripPanel, setShowTripPanel] = useState(false);
  const [selectedStopIndex, setSelectedStopIndex] = useState(0);

  const table = useTable<TripStopModel>({
    rows: tripsStopsReport,
    columns: [
      {
        labelId: "generic.arrival-date",
        row: (item) => (
          <div className="whitespace-nowrap h-full flex flex-col">
            <div className="flex-1">{show.dateTime(item.arrivalDate)}</div>
          </div>
        ),
        value: (item) => item.arrivalDate,
        sort: "desc"
      },
      {
        labelId: "generic.departure-date",
        row: (item) => (
          <div className="whitespace-nowrap h-full flex flex-col">
            <div className="flex-1">{show.dateTime(item.departureDate)}</div>
          </div>
        ),
        value: (item) => item.departureDate
      },
      {
        labelId: "generic.duration",
        row: (item) => <>{show.duration(item.duration)}</>,
        footer: () => <span className="font-semibold"></span>,
        value: (item) => item.duration
      },
      {
        labelId: "generic.location",
        row: (item) => (
          <div className="whitespace-nowrap text-ellipsis">
            <div>
              <GeocodeReverse coordinates={item.arrivalCoordinates} />
            </div>
          </div>
        )
      },
      {
        row: (item, index) => (
          <Button
            icon={faMagnifyingGlass}
            color="white"
            size="sm"
            onClick={(e) => {
              e.stopPropagation();
              setSelectedStopIndex(index);
              setShowTripPanel(true);
            }}
          />
        )
      }
    ]
  });

  const selectedStop = useMemo(() => {
    return table.props.rows?.[selectedStopIndex];
  }, [selectedStopIndex, table.props.rows]);

  const selectNextStop = () => {
    setSelectedStopIndex((prevIndex) =>
      prevIndex < (table.props.rows?.length ?? 0) - 1
        ? prevIndex + 1
        : prevIndex
    );
  };

  const selectPreviousStop = () => {
    setSelectedStopIndex((prevIndex) =>
      prevIndex > 0 ? prevIndex - 1 : prevIndex
    );
  };

  return (
    <>
      {selectedStop && (
        <AssetTripStopDetailsPanel
          stop={selectedStop}
          open={showTripPanel}
          close={() => setShowTripPanel(false)}
          previous={selectPreviousStop}
          next={selectNextStop}
          previousDisabled={selectedStopIndex === 0}
          nextDisabled={
            selectedStopIndex === (table.props.rows?.length ?? 0) - 1
          }
        />
      )}
      <LocationFilter configuration={filter.configuration} />
      <TableV2<TripStopModel> className="h-full" {...table.props} />
    </>
  );
}
