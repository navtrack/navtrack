import { LocationFilter } from "../shared/location-filter/LocationFilter";
import { useCurrentAsset } from "@navtrack/shared/hooks/current/useCurrentAsset";
import { TableV2 } from "../../ui/table/TableV2";
import { TripReportItem } from "@navtrack/shared/api/model";
import { useShow } from "@navtrack/shared/hooks/util/useShow";
import { useRecoilValue } from "recoil";
import { locationFiltersSelector } from "../shared/location-filter/locationFilterState";
import { useLocationFilterKey } from "../shared/location-filter/useLocationFilterKey";
import { useAssetReportTripsQuery } from "@navtrack/shared/hooks/queries/assets/useAssetReportTripsQuery";
import { Icon } from "../../ui/icon/Icon";
import { faArrowDown, faArrowUp } from "@fortawesome/free-solid-svg-icons";
import { GeocodeReverse } from "@navtrack/shared/components/components/geo/GeocodeReverse";

export function AssetReportsTripsPage() {
  const show = useShow();
  const currentAsset = useCurrentAsset();
  const locationFilterKey = useLocationFilterKey("reports-trips");
  const filters = useRecoilValue(locationFiltersSelector(locationFilterKey));
  const tripReport = useAssetReportTripsQuery({
    assetId: currentAsset.data?.id,
    startDate: filters.startDate,
    endDate: filters.endDate
  });

  return (
    <>
      <LocationFilter filterPage="reports-trips" />
      <TableV2<TripReportItem>
        className="h-full"
        columns={[
          {
            labelId: "generic.date",
            row: (item) => (
              <div className="whitespace-nowrap h-full flex flex-col">
                <div className="flex-1">
                  <Icon
                    icon={faArrowUp}
                    className="text-green-500 mr-1"
                    size="sm"
                  />
                  {show.dateTime(item.startPosition.date)}
                </div>
                <div className="flex-1">
                  <Icon
                    icon={faArrowDown}
                    className="text-red-500 mr-1"
                    size="sm"
                  />
                  {show.dateTime(item.endPosition.date)}
                </div>
              </div>
            ),
            footer: () => undefined,
            sortValue: (item) => item.startPosition.date,
            sortable: true
          },
          {
            labelId: "generic.location",
            row: (item) => (
              <div className="whitespace-nowrap text-ellipsis">
                <div className="">
                  <GeocodeReverse
                    coordinates={item.startPosition.coordinates}
                  />
                </div>
                <div>
                  <GeocodeReverse coordinates={item.endPosition.coordinates} />
                </div>
              </div>
            ),
            footer: () => undefined,
            rowClassName: "w-full"
          },
          {
            labelId: "generic.distance",
            row: (item) => <>{show.distance(item.distance) ?? "-"}</>,
            footer: () => (
              <span className="font-semibold">
                {show.distance(tripReport.data?.totalDistance)}
              </span>
            ),
            sortValue: (item) => item.distance,
            sortable: true
          },
          {
            labelId: "generic.duration",
            row: (item) => <>{show.duration(item.duration) ?? "-"}</>,
            footer: () => (
              <span className="font-semibold">
                {show.duration(tripReport.data?.totalDuration)}
              </span>
            ),
            sortValue: (item) => item.distance,
            sortable: true
          },
          {
            labelId: "generic.fuel-consumption",
            row: (item) => <>{show.volume(item.fuelConsumption) ?? "-"}</>,
            footer: () => (
              <span className="font-semibold">
                {show.volume(tripReport.data?.totalFuelConsumption)}
              </span>
            ),
            sortValue: (item) => item.fuelConsumption,
            sortable: true
          },
          {
            labelId: "generic.average-fuel-consumption",
            row: (item) => (
              <>{show.fuelConsumption(item.averageFuelConsumption) ?? "-"}</>
            ),
            footer: () => (
              <span className="font-semibold">
                {show.fuelConsumption(tripReport.data?.averageFuelConsumption)}
              </span>
            ),
            sortValue: (item) => item.averageFuelConsumption,
            sortable: true
          }
        ]}
        rows={tripReport.data?.items}
      />
    </>
  );
}
