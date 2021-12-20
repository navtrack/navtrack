import { ReactNode } from "react";
import { FormattedMessage } from "react-intl";

interface ILocationInfo {
  titleId: string;
  children: ReactNode | string;
  hideMargin?: boolean;
  className?: string;
}

export default function LocationInfo(props: ILocationInfo) {
  return (
    <div className={props.className}>
      <div className="text-xs">
        <FormattedMessage id={props.titleId} />
      </div>
      <div className="text-sm font-semibold">{props.children}</div>
    </div>
  );
}
