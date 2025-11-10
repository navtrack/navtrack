import { c, classNames } from "@navtrack/shared/utils/tailwind";
import { Icon } from "../../ui/icon/Icon";
import { faArrowDown, faArrowUp } from "@fortawesome/free-solid-svg-icons";
import { FormattedMessage } from "react-intl";
import { AssetStatsPeriod } from "@navtrack/shared/api/model";
import { Skeleton } from "../../../../../shared/src/components/components/ui/skeleton/Skeleton";
import { useChangePercentage } from "@navtrack/shared/hooks/util/useChangePercentage";
import { LoadingIndicator } from "@navtrack/shared/components/components/ui/loading-indicator/LoadingIndicator";

type DashboardItemProps = {
  period: AssetStatsPeriod;
  labelId: string;
  showFunction: (value: number | null | undefined) => string | undefined;
  mainValue?: number | null;
  secondaryValue?: number | null;
  isLoading?: boolean;
  isLoadingSecondary?: boolean;
};

const perivousPeriodLabelIds: Record<AssetStatsPeriod, string> = {
  [AssetStatsPeriod.Day]: "generic.yesterday",
  [AssetStatsPeriod.Week]: "generic.last-week",
  [AssetStatsPeriod.Month]: "generic.last-month",
  [AssetStatsPeriod.Year]: "generic.last-year"
};

export function DashboardItem(props: DashboardItemProps) {
  const changePercentage = useChangePercentage(
    props.mainValue,
    props.secondaryValue
  );

  return (
    <div className="p-6">
      <div className="flex justify-between">
        <div className="text-gray-900">
          <FormattedMessage id={props.labelId} />
        </div>
        <div>
          {!props.isLoading && props.isLoadingSecondary && <LoadingIndicator />}
        </div>
      </div>
      <div>
        <Skeleton isLoading={props.isLoading}>
          <div className="text-nowrap text-2xl font-semibold text-blue-600">
            {props.showFunction(props.mainValue) ?? "-"}
          </div>
          <div className="mt-1 flex text-nowrap text-sm font-medium text-gray-500">
            <div className="flex-1">
              <div className="text-sm">
                <FormattedMessage id={perivousPeriodLabelIds[props.period]} />
              </div>
              <div className="text-base">
                {props.showFunction(props.secondaryValue) ?? "-"}
              </div>
            </div>
            <div className="flex items-end">
              {!props.isLoading && (
                <div>
                  <div
                    className={classNames(
                      c(
                        changePercentage.direction === "increase",
                        "bg-green-100 text-green-800"
                      ),
                      c(
                        changePercentage.direction === "decrease",
                        "bg-red-100 text-red-800"
                      ),
                      c(
                        changePercentage.direction === undefined,
                        "bg-yellow-100 text-yellow-800"
                      ),
                      "inline-flex items-center space-x-1 rounded-full px-2.5 py-0.5 text-sm font-medium md:mt-2 lg:mt-0"
                    )}>
                    {changePercentage.direction !== undefined ? (
                      changePercentage.direction === "increase" ? (
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
                    <div>
                      {changePercentage.change !== undefined
                        ? `${changePercentage.change}%`
                        : "N/A"}
                    </div>
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
