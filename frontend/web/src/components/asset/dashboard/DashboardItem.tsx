import { classNames } from "@navtrack/shared/utils/tailwind";
import { Icon } from "../../ui/icon/Icon";
import { faArrowDown, faArrowUp } from "@fortawesome/free-solid-svg-icons";
import { FormattedMessage } from "react-intl";
import { AssetStatsDateRange } from "@navtrack/shared/api/model";
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
    <div className="p-6">
      <div className="text-gray-900">
        <FormattedMessage id={props.labelId} />
      </div>
      <div className="mt-1">
        <Skeleton isLoading={props.loading}>
          <div className="text-nowrap text-2xl font-semibold text-blue-600">
            {props.mainStat ?? "-"}
          </div>
          <div className="mt-1 flex text-nowrap text-sm font-medium text-gray-500">
            <div className="flex-1">
              <div className="text-sm">
                <FormattedMessage
                  id={previousDateRangeLabelIds[props.dateRange]}
                />
              </div>
              <div className="text-base">{props.secondaryStat ?? "-"}</div>
            </div>
            <div className="flex items-center">
              {!props.loading && (
                <div>
                  <div
                    className={classNames(
                      changeType === "increase"
                        ? "bg-green-100 text-green-800"
                        : "bg-red-100 text-red-800",
                      "inline-flex items-center space-x-1 rounded-full px-2.5 py-0.5 text-sm font-medium md:mt-2 lg:mt-0"
                    )}>
                    {props.change !== undefined ? (
                      changeType === "increase" ? (
                        <Icon
                          icon={faArrowUp}
                          className="text-green-500"
                          size="sm"
                        />
                      ) : (
                        <Icon
                          icon={faArrowDown}
                          className="text-red-500"
                          size="sm"
                        />
                      )
                    ) : null}
                    <div>{props.change ? `${props.change}%` : "N/A"}</div>
                  </div>
                </div>
              )}
            </div>
          </div>
        </Skeleton>
      </div>
    </div>
  );
}
