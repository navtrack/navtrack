import { useHistory, useRouteMatch } from "react-router-dom";
import { FormattedMessage } from "react-intl";
import { Icon } from "../ui/shared/icon/Icon";
import { IconProp } from "@fortawesome/fontawesome-svg-core";
import { classNames } from "@navtrack/shared/utils/tailwind";

export type SettingsSidebarItemProps = {
  label: string;
  href: string;
  icon: IconProp;
};

export function SettingsSidebarItem(props: SettingsSidebarItemProps) {
  const history = useHistory();
  const match = useRouteMatch(props.href);

  return (
    <div
      key={props.label}
      onClick={() => history.push(props.href)}
      className={classNames(
        match?.isExact
          ? "bg-gray-50 text-gray-700 hover:bg-white hover:text-gray-700"
          : "text-gray-900 hover:bg-gray-50 hover:text-gray-900",
        "group flex cursor-pointer items-center rounded-md px-3 py-2 text-sm font-medium"
      )}>
      <Icon
        icon={props.icon}
        className={classNames(
          match?.isExact
            ? "text-gray-900 group-hover:text-gray-900"
            : "text-gray-500 group-hover:text-gray-500",
          "-ml-1 mr-3 h-6 w-6 flex-shrink-0"
        )}
      />
      <span className="truncate">
        <FormattedMessage id={props.label} />
      </span>
    </div>
  );
}
