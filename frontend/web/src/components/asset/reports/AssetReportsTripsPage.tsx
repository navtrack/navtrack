import { LocationFilter } from "../shared/location-filter/LocationFilter";
import { useCurrentAsset } from "@navtrack/shared/hooks/current/useCurrentAsset";
import { TableV2 } from "../../ui/table/TableV2";
import { TripModel } from "@navtrack/shared/api/model";
import { useShow } from "@navtrack/shared/hooks/util/useShow";
import { useAssetTripsQueries } from "@navtrack/shared/hooks/queries/assets/useAssetTripsQueries";
import { Icon } from "../../ui/icon/Icon";
import { faArrowDown, faArrowUp } from "@fortawesome/free-solid-svg-icons";
import { GeocodeReverse } from "@navtrack/shared/components/components/geo/GeocodeReverse";
import { useTable } from "../../ui/table/useTable";
import { useLocationFilter } from "../shared/location-filter/useLocationFilter";

export function AssetReportsTripsPage() {
  const show = useShow();
  const currentAsset = useCurrentAsset();
  const filter = useLocationFilter({
    page: "asset-reports-trips"
  });
  const query = useAssetTripsQueries({
    assetId: currentAsset.data?.id,
    ...filter.filters
  });

  const table = useTable<TripModel>({
    rows: query.allTrips,
    columns: [
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
        value: (item) => item.startPosition.date
      },
      {
        labelId: "generic.location",
        row: (item) => (
          <div className="whitespace-nowrap text-ellipsis">
            <div className="">
              <GeocodeReverse coordinates={item.startPosition.coordinates} />
            </div>
            <div>
              <GeocodeReverse coordinates={item.endPosition.coordinates} />
            </div>
          </div>
        ),
        rowClassName: "w-full"
      },
      {
        labelId: "generic.distance",
        row: (item) => <>{show.distance(item.distance) ?? "-"}</>,
        footer: () => (
          <span className="font-semibold">
            {show.distance(query.totalDistance)}
          </span>
        ),
        value: (item) => item.distance
      },
      {
        labelId: "generic.duration",
        row: (item) => <>{show.duration(item.duration) ?? "-"}</>,
        footer: () => (
          <span className="font-semibold">
            {show.duration(query.totalDuration)}
          </span>
        ),
        value: (item) => item.distance
      },
      {
        labelId: "generic.fuel-consumption",
        row: (item) => <>{show.volume(item.fuelConsumption) ?? "-"}</>,
        footer: () => (
          <span className="font-semibold">
            {show.volume(query.totalFuelConsumption)}
          </span>
        ),
        value: (item) => item.fuelConsumption
      },
      {
        labelId: "generic.average-fuel-consumption",
        row: (item) => (
          <>{show.fuelConsumption(item.averageFuelConsumption) ?? "-"}</>
        ),
        footer: () => (
          <span className="font-semibold">
            {show.fuelConsumption(query.averageFuelConsumption)}
          </span>
        ),
        value: (item) => item.averageFuelConsumption
      }
    ]
  });

  return (
    <>
      <LocationFilter configuration={filter.configuration} />
      <TableV2<TripModel> className="h-full" {...table.props} />
    </>
  );
}
