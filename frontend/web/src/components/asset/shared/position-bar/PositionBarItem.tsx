import { FormattedMessage } from "react-intl";
import { ReactNode } from "react";
import { Skeleton } from "../../../ui/skeleton/Skeleton";

type PositionBarItemProps = {
  label: string;
  value: ReactNode;
  loading?: boolean;
};

export function PositionBarItem(props: PositionBarItemProps) {
  return (
    <div>
      <div className="text-xs font-medium uppercase tracking-wider text-gray-500">
        <FormattedMessage id={props.label} />
      </div>
      <Skeleton loading={props.loading ?? false}>
        <div className="text-sm font-medium">{props.value}</div>
      </Skeleton>
    </div>
  );
}
