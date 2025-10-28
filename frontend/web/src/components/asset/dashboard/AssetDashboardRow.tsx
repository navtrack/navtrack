import { useAssetStatsQuery } from "@navtrack/shared/hooks/queries/assets/useAssetStatsQuery";
import { DashboardItem } from "./DashboardItem";
import { useCurrentAsset } from "@navtrack/shared/hooks/current/useCurrentAsset";
import { FormattedMessage } from "react-intl";
import { useShow } from "@navtrack/shared/hooks/util/useShow";
import { Card } from "../../ui/card/Card";
import { AssetStatsPeriod } from "@navtrack/shared/api/model";

type AssetDashboardRowProps = {
  period: AssetStatsPeriod;
  labelId: string;
};

export function AssetDashboardRow(props: AssetDashboardRowProps) {
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
            showFunction={(value) => show.distance(value, true)}
            mainValue={statsQuery.data?.distance}
            secondaryValue={statsQuery.data?.distancePrevious}
            isLoading={statsQuery.isLoading}
          />
          <DashboardItem
            period={props.period}
            labelId="generic.duration"
            showFunction={show.duration}
            mainValue={statsQuery.data?.duration}
            secondaryValue={statsQuery.data?.durationPrevious}
            isLoading={statsQuery.isLoading}
          />
          <DashboardItem
            period={props.period}
            labelId="generic.fuel-consumed"
            showFunction={show.volume}
            mainValue={statsQuery.data?.fuelConsumption}
            secondaryValue={statsQuery.data?.fuelConsumptionPrevious}
            isLoading={statsQuery.isLoading}
          />
          <DashboardItem
            period={props.period}
            labelId="generic.average-fuel-consumption"
            showFunction={show.fuelConsumption}
            mainValue={statsQuery.data?.fuelConsumptionAverage}
            secondaryValue={statsQuery.data?.fuelConsumptionAveragePrevious}
            isLoading={statsQuery.isLoading}
          />
        </dl>
      </Card>
    </div>
  );
}
