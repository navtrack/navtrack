import { useAssetStatsQuery } from "@navtrack/shared/hooks/queries/assets/useAssetStatsQuery";
import { DashboardItem } from "./DashboardItem";
import { useCurrentAsset } from "@navtrack/shared/hooks/current/useCurrentAsset";
import { FormattedMessage } from "react-intl";
import { useShow } from "@navtrack/shared/hooks/util/useShow";
import { Card } from "../../ui/card/Card";
import { AssetStatsPeriod } from "@navtrack/shared/api/model";

type DashboardRowProps = {
  period: AssetStatsPeriod;
  labelId: string;
};

export function DashboardRow(props: DashboardRowProps) {
  const currentAsset = useCurrentAsset();
  const show = useShow();
  const statsQuery = useAssetStatsQuery({
    assetId: currentAsset.data?.id,
    period: props.period
  });

  return (
    <div>
      <h3 className="text-lg font-semibold leading-6 text-gray-900">
        <FormattedMessage id={props.labelId} />
      </h3>
      <Card>
        <dl className="mt-2 grid grid-cols-1 divide-y divide-gray-200 overflow-hidden md:grid-cols-4 md:divide-x md:divide-y-0">
          <DashboardItem
            period={props.period}
            labelId="generic.distance"
            mainStat={show.distance(statsQuery.data?.distance, true)}
            secondaryStat={show.distance(
              statsQuery.data?.distancePrevious,
              true
            )}
            change={statsQuery.data?.distanceChange}
            loading={statsQuery.isLoading}
          />
          <DashboardItem
            period={props.period}
            labelId="generic.duration"
            mainStat={show.duration(statsQuery.data?.duration)}
            secondaryStat={show.duration(statsQuery.data?.durationPrevious)}
            change={statsQuery.data?.durationChange}
            loading={statsQuery.isLoading}
          />
          <DashboardItem
            period={props.period}
            labelId="generic.fuel-consumed"
            mainStat={show.volume(statsQuery.data?.fuelConsumption)}
            secondaryStat={show.volume(
              statsQuery.data?.fuelConsumptionPrevious
            )}
            change={statsQuery.data?.fuelConsumptionChange}
            loading={statsQuery.isLoading}
          />
          <DashboardItem
            period={props.period}
            labelId="generic.average-fuel-consumption"
            mainStat={show.fuelConsumption(
              statsQuery.data?.fuelConsumptionAverage
            )}
            secondaryStat={show.fuelConsumption(
              statsQuery.data?.fuelConsumptionAveragePrevious
            )}
            change={statsQuery.data?.fuelConsumptionAverageChange}
            loading={statsQuery.isLoading}
          />
        </dl>
      </Card>
    </div>
  );
}
