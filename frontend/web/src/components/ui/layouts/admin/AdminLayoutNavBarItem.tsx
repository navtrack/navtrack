import { IconProp } from "@fortawesome/fontawesome-svg-core";
import { FormattedMessage } from "react-intl";
import { IconWithText } from "../../shared/icon/IconWithText";
import { useHistory, useRouteMatch } from "react-router-dom";
import { classNames } from "@navtrack/shared/utils/tailwind";

type AdminLayoutNavBarItemProps = {
  labelId: string;
  icon: IconProp;
  href: string;
};

export function AdminLayoutNavBarItem(props: AdminLayoutNavBarItemProps) {
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
        match ? "border-gray-900 text-gray-900 hover:border-gray-500" : null,
        "flex cursor-pointer items-center border-b-2 border-transparent px-1 pt-1 text-sm font-medium text-gray-500 hover:border-gray-300 hover:text-gray-700"
      )}>
      <IconWithText icon={props.icon}>
        <FormattedMessage id={props.labelId} />
      </IconWithText>
    </a>
  );
}
