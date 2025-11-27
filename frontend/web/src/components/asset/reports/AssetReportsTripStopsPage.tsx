import { LocationFilter } from "../shared/location-filter/LocationFilter";
import { useCurrentAsset } from "@navtrack/shared/hooks/current/useCurrentAsset";
import { TableV2 } from "../../ui/table/TableV2";
import { useShow } from "@navtrack/shared/hooks/util/useShow";
import { useAtomValue } from "jotai";
import { locationFiltersSelector } from "../shared/location-filter/locationFilterState";
import { useLocationFilterKey } from "../shared/location-filter/useLocationFilterKey";
import { GeocodeReverse } from "@navtrack/shared/components/components/geo/GeocodeReverse";
import {
  TripStopModel,
  useAssetTripsStopsQueries
} from "@navtrack/shared/hooks/queries/assets/useAssetTripsStopsQueries";
import { Button } from "../../ui/button/Button";
import { faMagnifyingGlass } from "@fortawesome/free-solid-svg-icons";
import { useMemo, useState } from "react";
import { AssetTripStopDetailsPanel } from "./AssetTripStopDetailsPanel";

export function AssetReportsTripStopsPage() {
  const show = useShow();
  const currentAsset = useCurrentAsset();
  const locationFilterKey = useLocationFilterKey("reports-trips");
  const filters = useAtomValue(locationFiltersSelector(locationFilterKey));
  const tripsStopsReport = useAssetTripsStopsQueries({
    assetId: currentAsset.data?.id,
    startDate: filters.startDate,
    endDate: filters.endDate
  });
  const [showTripPanel, setShowTripPanel] = useState(false);
  const [selectedStopIndex, setSelectedStopIndex] = useState(0);

  const selectedStop = useMemo(() => {
    return tripsStopsReport[selectedStopIndex];
  }, [selectedStopIndex, tripsStopsReport]);

  const selectNextStop = () => {
    setSelectedStopIndex((prevIndex) =>
      prevIndex < tripsStopsReport.length - 1 ? prevIndex + 1 : prevIndex
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
          nextDisabled={selectedStopIndex === tripsStopsReport.length - 1}
        />
      )}
      <LocationFilter filterPage="reports-trips" />
      <TableV2<TripStopModel>
        className="h-full"
        columns={[
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
                <div className="flex-1">
                  {show.dateTime(item.departureDate)}
                </div>
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
        ]}
        rows={tripsStopsReport}
      />
    </>
  );
}
