import { classNames } from "@navtrack/shared/utils/tailwind";
import { FormattedMessage } from "react-intl";
import { Link as RouterLink } from "react-router-dom";

export type LinkProps = {
  to: string;
  label: string;
  className?: string;
};

export function Link(props: LinkProps) {
  return (
    <RouterLink
      to={props.to}
      className={classNames(
        props.className,
        "text-indigo-600 hover:text-indigo-900"
      )}>
      <FormattedMessage id={props.label} />
    </RouterLink>
  );
}
