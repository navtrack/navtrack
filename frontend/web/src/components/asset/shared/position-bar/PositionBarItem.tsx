import { FormattedMessage } from "react-intl";
import { ReactNode } from "react";

export function PositionBarItem(props: { label: string; value: ReactNode }) {
  return (
    <div>
      <div className="text-xs font-medium uppercase tracking-wider text-gray-500">
        <FormattedMessage id={props.label} />
      </div>
      <div className="text-sm font-medium">{props.value}</div>
    </div>
  );
}
