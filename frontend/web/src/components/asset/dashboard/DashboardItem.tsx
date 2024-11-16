import { classNames } from "@navtrack/shared/utils/tailwind";
import { Icon } from "../../ui/icon/Icon";
import { faArrowDown, faArrowUp } from "@fortawesome/free-solid-svg-icons";
import { FormattedMessage } from "react-intl";
import { AssetStatsDateRange } from "@navtrack/shared/api/model/generated";
import { Skeleton } from "../../ui/skeleton/Skeleton";

type DashboardItemProps = {
  dateRange: AssetStatsDateRange;
  labelId: string;
  mainStat?: string;
  secondaryStat?: string;
  change?: number | null;
  loading?: boolean;
};

const previousDateRangeLabelIds: Record<AssetStatsDateRange, string> = {
  [AssetStatsDateRange.Today]: "generic.yesterday",
  [AssetStatsDateRange.ThisWeek]: "generic.last-week",
  [AssetStatsDateRange.ThisMonth]: "generic.last-month",
  [AssetStatsDateRange.ThisYear]: "generic.last-year"
};

export function DashboardItem(props: DashboardItemProps) {
  const changeType = (props.change ?? 0) < 0 ? "decrease" : "increase";

  return (
    <div className="px-4 py-5 sm:p-6">
      <div className="flex justify-between">
        <dt className="text-base font-normal text-gray-900">
          <FormattedMessage id={props.labelId} />
        </dt>
        {!props.loading && (
          <div
            className={classNames(
              changeType === "increase"
                ? "bg-green-100 text-green-800"
                : "bg-red-100 text-red-800",
              "inline-flex items-center space-x-1 rounded-full px-2.5 py-0.5 text-sm font-medium md:mt-2 lg:mt-0"
            )}>
            {props.change !== undefined ? (
              changeType === "increase" ? (
                <Icon icon={faArrowUp} className="text-green-500" size="sm" />
              ) : (
                <Icon icon={faArrowDown} className="text-red-500" size="sm" />
              )
            ) : null}
            <div>{props.change ? `${props.change}%` : "N/A"}</div>
          </div>
        )}
      </div>
      <Skeleton isLoading={props.loading} className="mt-1">
        <dd className="flex items-end py-1">
          <div>
            <span className="text-nowrap border-r-2 pr-2 text-2xl font-semibold text-indigo-600">
              {props.mainStat ?? <FormattedMessage id="generic.na" />}
            </span>
            <span className="ml-2 space-x-1 text-nowrap text-sm text-gray-500">
              <span className="font-medium">
                {props.secondaryStat ?? <FormattedMessage id="generic.na" />}
              </span>
              <span className="font-normal lowercase">
                <FormattedMessage
                  id={previousDateRangeLabelIds[props.dateRange]}
                />
              </span>
            </span>
          </div>
        </dd>
      </Skeleton>
    </div>
  );
}
