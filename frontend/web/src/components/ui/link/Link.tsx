import { classNames } from "@navtrack/shared/utils/tailwind";
import { FormattedMessage } from "react-intl";
import { Link as RouterLink } from "react-router-dom";

export type LinkProps = {
  to: string;
  label: string;
  className?: string;
  onClick?: () => void;
};

export function Link(props: LinkProps) {
  return (
    <RouterLink
      onClick={props.onClick}
      to={props.to}
      className={classNames(
        props.className,
        "text-blue-700 hover:text-blue-800"
      )}>
      <FormattedMessage id={props.label} />
    </RouterLink>
  );
}
