import { useAssetStatsQuery } from "@navtrack/shared/hooks/queries/assets/useAssetStatsQuery";
import { DashboardItem } from "./DashboardItem";
import { useCurrentAsset } from "@navtrack/shared/hooks/current/useCurrentAsset";
import { FormattedMessage } from "react-intl";
import { AssetStatsDateRange } from "@navtrack/shared/api/model";
import { useMemo } from "react";
import { useShow } from "@navtrack/shared/hooks/util/useShow";
import { Card } from "../../ui/card/Card";

const dateRanges = [
  { dataRange: AssetStatsDateRange.Today, labelId: "generic.today" },
  { dataRange: AssetStatsDateRange.ThisWeek, labelId: "generic.this-week" },
  { dataRange: AssetStatsDateRange.ThisMonth, labelId: "generic.this-month" },
  { dataRange: AssetStatsDateRange.ThisYear, labelId: "generic.this-year" }
];

export function AssetDashboardPage() {
  const currentAsset = useCurrentAsset();
  const show = useShow();

  const statsQuery = useAssetStatsQuery({
    assetId: currentAsset.data?.id
  });

  const stats = useMemo(
    () =>
      dateRanges.map((x) => ({
        ...x,
        data: statsQuery.data?.items.find((y) => y.dateRange === x.dataRange)
      })),
    [statsQuery.data?.items]
  );

  return (
    <>
      {stats.map((item) => (
        <div key={item.labelId}>
          <h3 className="text-lg font-semibold leading-6 text-gray-900">
            <FormattedMessage id={item.labelId} />
          </h3>
          <Card>
            <dl className="mt-2 grid grid-cols-1 divide-y divide-gray-200 overflow-hidden md:grid-cols-4 md:divide-x md:divide-y-0">
              <DashboardItem
                dateRange={item.dataRange}
                labelId="generic.distance"
                mainStat={show.distance(item.data?.distance, true)}
                secondaryStat={show.distance(item.data?.distancePrevious, true)}
                change={item.data?.distanceChange}
                loading={statsQuery.isLoading}
              />
              <DashboardItem
                dateRange={item.dataRange}
                labelId="generic.duration"
                mainStat={show.duration(item.data?.duration)}
                secondaryStat={show.duration(item.data?.durationPrevious)}
                change={item.data?.durationChange}
                loading={statsQuery.isLoading}
              />
              <DashboardItem
                dateRange={item.dataRange}
                labelId="generic.fuel-consumed"
                mainStat={show.volume(item.data?.fuelConsumption)}
                secondaryStat={show.volume(item.data?.fuelConsumptionPrevious)}
                change={item.data?.fuelConsumptionChange}
                loading={statsQuery.isLoading}
              />
              <DashboardItem
                dateRange={item.dataRange}
                labelId="generic.average-fuel-consumption"
                mainStat={show.fuelConsumption(
                  item.data?.fuelConsumptionAverage
                )}
                secondaryStat={show.fuelConsumption(
                  item.data?.fuelConsumptionAveragePrevious
                )}
                change={item.data?.fuelConsumptionAverageChange}
                loading={statsQuery.isLoading}
              />
            </dl>
          </Card>
        </div>
      ))}
    </>
  );
}
