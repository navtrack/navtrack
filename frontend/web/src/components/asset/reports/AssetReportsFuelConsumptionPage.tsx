import { useAssetReportFuelConsumptionQuery } from "@navtrack/shared/hooks/queries/assets/useAssetReportFuelConsumptionQuery";
import { LocationFilter } from "../shared/location-filter/LocationFilter";
import { useCurrentAsset } from "@navtrack/shared/hooks/current/useCurrentAsset";
import { TableV2 } from "../../ui/table/TableV2";
import { FuelConsumptionReportItemModel } from "@navtrack/shared/api/model";
import { useShow } from "@navtrack/shared/hooks/util/useShow";
import { FormattedMessage } from "react-intl";
import { useAtomValue } from "jotai";
import { locationFiltersSelector } from "../shared/location-filter/locationFilterState";
import { useLocationFilterKey } from "../shared/location-filter/useLocationFilterKey";

export function AssetReportsFuelConsumptionPage() {
  const show = useShow();
  const currentAsset = useCurrentAsset();
  const locationFilterKey = useLocationFilterKey("reports-fuel-consumption");
  const filters = useAtomValue(locationFiltersSelector(locationFilterKey));
  const fuelConsumptionReport = useAssetReportFuelConsumptionQuery({
    assetId: currentAsset.data?.id,
    startDate: filters.startDate,
    endDate: filters.endDate
  });

  return (
    <>
      <LocationFilter filterPage="reports-fuel-consumption" />
      <TableV2<FuelConsumptionReportItemModel>
        className="h-full"
        columns={[
          {
            labelId: "generic.date",
            row: (item) => <>{show.date(item.date)}</>,
            footer: () => <FormattedMessage id="generic.total" />,
            sortValue: (item) => item.date,
            sortable: true,
            sort: "desc"
          },
          {
            labelId: "generic.distance",
            row: (item) => <>{show.distance(item.distance) ?? "-"}</>,
            footer: () => (
              <span className="font-semibold">
                {show.distance(fuelConsumptionReport.data?.totalDistance)}
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
                {show.duration(fuelConsumptionReport.data?.totalDuration)}
              </span>
            ),
            sortValue: (item) => item.duration,
            sortable: true
          },
          {
            labelId: "generic.average-speed",
            row: (item) => <>{show.speed(item.averageSpeed) ?? "-"}</>,
            footer: () => (
              <span className="font-semibold">
                {show.speed(fuelConsumptionReport.data?.averageSpeed)}
              </span>
            ),
            sortValue: (item) => item.averageSpeed,
            sortable: true
          },
          {
            labelId: "generic.fuel-consumption",
            row: (item) => <>{show.volume(item.fuelConsumption) ?? "-"}</>,
            footer: () => (
              <span className="font-semibold">
                {show.volume(fuelConsumptionReport.data?.totalFuelConsumption)}
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
                {show.fuelConsumption(
                  fuelConsumptionReport.data?.averageFuelConsumption
                )}
              </span>
            ),
            sortValue: (item) => item.averageFuelConsumption,
            sortable: true
          }
        ]}
        rows={fuelConsumptionReport.data?.items}
      />
    </>
  );
}
