import { FormattedMessage } from "react-intl";
import { ReactNode } from "react";
import { Skeleton } from "../../../../../../shared/src/components/components/ui/skeleton/Skeleton";
import { Copy } from "../../../ui/helpers/Copy";

type PositionCardItemProps = {
  label: string;
  labelExtra?: ReactNode;
  value: ReactNode;
  isLoading?: boolean;
  copyable?: boolean;
  className?: string;
};

export function PositionCardItem(props: PositionCardItemProps) {
  return (
    <div className={props.className}>
      <div className="text-xs font-medium uppercase tracking-wider text-gray-500">
        <FormattedMessage id={props.label} />
        {props.copyable && props.value && (
          <Copy className="ml-2" value={props.value.toString()} />
        )}
        {props.labelExtra}
      </div>
      <Skeleton isLoading={props.isLoading ?? false}>
        <div className="text-sm font-medium">{props.value}</div>
      </Skeleton>
    </div>
  );
}
