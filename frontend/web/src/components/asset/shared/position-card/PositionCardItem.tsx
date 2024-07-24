import { FormattedMessage } from "react-intl";
import { ReactNode } from "react";
import { Skeleton } from "../../../ui/skeleton/Skeleton";

type PositionCardItemProps = {
  label: string;
  value: ReactNode;
  isLoading?: boolean;
};

export function PositionCardItem(props: PositionCardItemProps) {
  return (
    <div>
      <div className="text-xs font-medium uppercase tracking-wider text-gray-500">
        <FormattedMessage id={props.label} />
      </div>
      <Skeleton loading={props.isLoading ?? false}>
        <div className="text-sm font-medium">{props.value}</div>
      </Skeleton>
    </div>
  );
}
