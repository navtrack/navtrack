import classNames from "classnames";
import { IconProp } from "@fortawesome/fontawesome-svg-core";
import { FormattedMessage } from "react-intl";
import IconWithText from "../../shared/icon/IconWithText";
import { useHistory, useRouteMatch } from "react-router";

interface IAdminLayoutNavBarItem {
  labelId: string;
  icon: IconProp;
  href: string;
}

export default function AdminLayoutNavBarItem(props: IAdminLayoutNavBarItem) {
  const history = useHistory();
  const match = useRouteMatch({ path: props.href });

  return (
    <a
      href={props.href}
      onClick={(e) => {
        e.preventDefault();
        history.push(props.href);
      }}
      className={classNames(
        match ? "border-gray-900 hover:border-gray-500 text-gray-900" : null,
        "cursor-pointer border-transparent text-gray-500 hover:border-gray-300 hover:text-gray-700 flex items-center px-1 pt-1 border-b-2 text-sm font-medium"
      )}>
      <IconWithText icon={props.icon}>
        <FormattedMessage id={props.labelId} />
      </IconWithText>
    </a>
  );
}
