import { FormattedMessage } from "react-intl";
import { useShow } from "@navtrack/shared/hooks/util/useShow";
import { Card } from "../../ui/card/Card";
import { AssetStatsPeriod } from "@navtrack/shared/api/model";
import { useAssetsQuery } from "@navtrack/shared/hooks/queries/assets/useAssetsQuery";
import { useCurrentOrganization } from "@navtrack/shared/hooks/current/useCurrentOrganization";
import { useAssetStatsQueries } from "@navtrack/shared/hooks/queries/assets/useAssetStatsQueries";
import { useMemo } from "react";
import { DashboardItem } from "../../asset/dashboard/DashboardItem";

type OrganizationDashboardRowProps = {
  period: AssetStatsPeriod;
  labelId: string;
};

export function OrganizationDashboardRow(props: OrganizationDashboardRowProps) {
  const show = useShow();
  const currentOrganization = useCurrentOrganization();
  const assets = useAssetsQuery({
    organizationId: currentOrganization.data?.id
  });
  const statsQueries = useAssetStatsQueries({
    assetIds: assets.data?.items.map((asset) => asset.id) || [],
    period: props.period
  });

  const stats = useMemo(
    () => ({
      distance: statsQueries.reduce(
        (sum, query) => sum + (query.data?.distance || 0),
        0
      ),
      distancePrevious: statsQueries.reduce(
        (sum, query) => sum + (query.data?.distancePrevious || 0),
        0
      ),
      duration: statsQueries.reduce(
        (sum, query) => sum + (query.data?.duration || 0),
        0
      ),
      durationPrevious: statsQueries.reduce(
        (sum, query) => sum + (query.data?.durationPrevious || 0),
        0
      ),
      fuelConsumption: statsQueries.reduce(
        (sum, query) => sum + (query.data?.fuelConsumption || 0),
        0
      ),
      fuelConsumptionPrevious: statsQueries.reduce(
        (sum, query) => sum + (query.data?.fuelConsumptionPrevious || 0),
        0
      ),
      fuelConsumptionAverage: statsQueries.reduce(
        (sum, query) => sum + (query.data?.fuelConsumptionAverage || 0),
        0
      ),
      fuelConsumptionAveragePrevious: statsQueries.reduce(
        (sum, query) => sum + (query.data?.fuelConsumptionAveragePrevious || 0),
        0
      )
    }),
    [statsQueries]
  );

  const isLoading = useMemo(
    () => statsQueries.some((query) => query.isLoading),
    [statsQueries]
  );

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
            mainValue={stats.distance}
            secondaryValue={stats.distancePrevious}
            isLoadingSecondary={isLoading}
          />
          <DashboardItem
            period={props.period}
            labelId="generic.duration"
            showFunction={show.duration}
            mainValue={stats.duration}
            secondaryValue={stats.durationPrevious}
            isLoadingSecondary={isLoading}
          />
          <DashboardItem
            period={props.period}
            labelId="generic.fuel-consumed"
            showFunction={show.volume}
            mainValue={stats.fuelConsumption}
            secondaryValue={stats.fuelConsumptionPrevious}
            isLoadingSecondary={isLoading}
          />
          <DashboardItem
            period={props.period}
            labelId="generic.average-fuel-consumption"
            showFunction={show.fuelConsumption}
            mainValue={stats.fuelConsumptionAverage}
            secondaryValue={stats.fuelConsumptionAveragePrevious}
            isLoadingSecondary={isLoading}
          />
        </dl>
      </Card>
    </div>
  );
}
