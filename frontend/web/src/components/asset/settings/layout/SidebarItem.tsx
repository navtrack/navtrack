import { Icon } from "../../../ui/shared/icon/Icon";
import { useHistory, useRouteMatch } from "react-router-dom";
import { IconProp } from "@fortawesome/fontawesome-svg-core";
import { FormattedMessage } from "react-intl";
import { classNames } from "@navtrack/shared/utils/tailwind";

export type SidebarItemProps = {
  label: string;
  href: string;
  icon: IconProp;
};

export function SidebarItem(props: SidebarItemProps) {
  const history = useHistory();
  const match = useRouteMatch(props.href);

  return (
    <a
      href={props.href}
      className={classNames(
        match?.isExact
          ? "border-teal-500 bg-teal-50 text-teal-700 hover:bg-teal-50 hover:text-teal-700"
          : "border-transparent text-gray-900 hover:bg-gray-50 hover:text-gray-900",
        "group flex items-center border-l-4 px-3 py-2 text-sm font-medium"
      )}
      onClick={(e) => {
        e.preventDefault();
        history.push(props.href);
      }}>
      <Icon
        className={classNames(
          match?.isExact
            ? "text-teal-500 group-hover:text-teal-500"
            : "text-gray-400 group-hover:text-gray-500",
          "-ml-1 mr-3 h-6 w-6 flex-shrink-0"
        )}
        icon={props.icon}
      />
      <span className="truncate">
        <FormattedMessage id={props.label} />
      </span>
    </a>
  );
}
