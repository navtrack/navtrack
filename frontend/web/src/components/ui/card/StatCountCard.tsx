import { ReactNode } from "react";
import { Card } from "./Card";
import { FormattedMessage } from "react-intl";
import { classNames } from "@navtrack/shared/utils/tailwind";
import { Skeleton } from "../skeleton/Skeleton";

export type StatCountCardProps = {
  labelId: string;
  count: ReactNode;
  countClassName?: string;
  totalCount?: ReactNode;
  loading?: boolean;
};

export function StatCountCard(props: StatCountCardProps) {
  return (
    <Card className="p-4">
      <div
        className={classNames(
          "truncate text-sm font-medium text-gray-500",
          props.countClassName
        )}>
        <FormattedMessage id={props.labelId} />
      </div>
      <div className="mt-1 text-3xl font-semibold tracking-tight text-gray-900">
        <Skeleton isLoading={props.loading}>
          {props.count}
          {props.totalCount !== undefined && (
            <span className="ml-2 text-sm font-medium text-gray-500">
              <FormattedMessage
                id="ui.card.stat.from"
                values={{ count: props.totalCount }}
              />
            </span>
          )}
        </Skeleton>
      </div>
    </Card>
  );
}
