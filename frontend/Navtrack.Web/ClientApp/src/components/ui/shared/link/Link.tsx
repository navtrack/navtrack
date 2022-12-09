import classNames from "classnames";
import { FormattedMessage } from "react-intl";
import { Link as RouterLink } from "react-router-dom";

export interface ILinkProps {
  to: string;
  text: string;
  className?: string;
}

export default function Link(props: ILinkProps) {
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
