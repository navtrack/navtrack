import { useAssetStatsQuery } from "@navtrack/shared/hooks/queries/useAssetStatsQuery";
import { DashboardItem } from "./DashboardItem";
import { useDistance } from "@navtrack/shared/hooks/util/useDistance";
import { useCurrentAsset } from "@navtrack/shared/hooks/assets/useCurrentAsset";
import { FormattedMessage } from "react-intl";
import { AssetStatsDateRange } from "@navtrack/shared/api/model/generated";
import { useDateTime } from "@navtrack/shared/hooks/util/useDateTime";

const dateRangeLabelIds: Record<AssetStatsDateRange, string> = {
  [AssetStatsDateRange.Today]: "generic.today",
  [AssetStatsDateRange.ThisWeek]: "generic.this-week",
  [AssetStatsDateRange.ThisMonth]: "generic.this-month",
  [AssetStatsDateRange.ThisYear]: "generic.this-year"
};

export function AssetDashboardPage() {
  const currentAsset = useCurrentAsset();
  const distance = useDistance();
  const dateTime = useDateTime();

  const statsQuery = useAssetStatsQuery({
    assetId: currentAsset.data?.id
  });

  return (
    <>
      {statsQuery.data?.items.map((item) => (
        <div>
          <h3 className="text-lg font-semibold leading-6 text-gray-900">
            {item.dateRange && (
              <FormattedMessage id={dateRangeLabelIds[item.dateRange]} />
            )}
          </h3>
          <dl className="mt-2 grid grid-cols-1 divide-y divide-gray-200 overflow-hidden rounded-lg bg-white shadow md:grid-cols-3 md:divide-x md:divide-y-0">
            <DashboardItem
              dateRange={item.dateRange}
              labelId="generic.distance"
              currentStat={distance.showDistance(
                item.distance ?? undefined,
                true
              )}
              previousStat={distance.showDistance(
                item.distancePrevious ?? undefined,
                true
              )}
              change={item.distanceChange}
            />
            <DashboardItem
              dateRange={item.dateRange}
              labelId="generic.duration"
              currentStat={dateTime.showDuration(item.duration ?? 0)}
              previousStat={dateTime.showDuration(item.durationPrevious ?? 0)}
              change={item.durationChange}
            />
            <DashboardItem
              dateRange={item.dateRange}
              labelId="generic.fuel-consumption"
              currentStat={`${item.fuelConsumption} l`}
              previousStat={`${item.fuelConsumptionPrevious} l`}
              change={item.fuelConsumptionChange}
            />
          </dl>
        </div>
      ))}
    </>
  );
}
