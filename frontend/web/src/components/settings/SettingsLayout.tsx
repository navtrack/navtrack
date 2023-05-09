import classNames from "classnames";
import { ReactNode } from "react";
import { useHistory, useRouteMatch } from "react-router";
import { FormattedMessage } from "react-intl";
import { IconProp } from "@fortawesome/fontawesome-svg-core";
import { Icon } from "../ui/shared/icon/Icon";
import { faUser } from "@fortawesome/free-regular-svg-icons";
import { faKey } from "@fortawesome/free-solid-svg-icons";
import Paths from "../../app/Paths";

interface ISettingsSidebarItem {
  label: string;
  href: string;
  icon: IconProp;
}

function SettingsSidebarItem(props: ISettingsSidebarItem) {
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

const menuItems: ISettingsSidebarItem[] = [
  {
    label: "settings.menu.account",
    href: Paths.SettingsAccount,
    icon: faUser
  },
  {
    label: "settings.menu.password",
    href: Paths.SettingsPassword,
    icon: faKey
  }
];

export interface ISettingsLayout {
  children?: ReactNode;
}

export function SettingsLayout(props: ISettingsLayout) {
  return (
    <div className="grid grid-cols-12 gap-x-5">
      <div className="py-6 px-2 sm:px-6 lg:col-span-3 lg:py-0 lg:px-0">
        <div className="space-y-1">
          {menuItems.map((item) => (
            <SettingsSidebarItem
              key={item.href}
              label={item.label}
              icon={item.icon}
              href={item.href}
            />
          ))}
        </div>
      </div>
      <div className="space-y-6 sm:px-6 lg:col-span-9 lg:px-0">
        {props.children}
      </div>
    </div>
  );
}
