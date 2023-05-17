import { classNames } from "@navtrack/shared/utils/tailwind";
import { FormattedMessage } from "react-intl";
import { Link as RouterLink } from "react-router-dom";

export type LinkProps = {
  to: string;
  text: string;
  className?: string;
};

export function Link(props: LinkProps) {
  return (
    <RouterLink
      to={props.to}
      className={classNames(
        props.className,
        "text-blue-600 hover:text-blue-900"
      )}>
      <FormattedMessage id={props.text} />
    </RouterLink>
  );
}
