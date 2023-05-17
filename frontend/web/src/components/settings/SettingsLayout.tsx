import { ReactNode } from "react";
import { faUser } from "@fortawesome/free-regular-svg-icons";
import { faKey } from "@fortawesome/free-solid-svg-icons";
import Paths from "../../app/Paths";
import {
  SettingsSidebarItem,
  SettingsSidebarItemProps
} from "./SettingsSidebarItem";

const menuItems: SettingsSidebarItemProps[] = [
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

type SettingsLayoutProps = {
  children?: ReactNode;
};

export function SettingsLayout(props: SettingsLayoutProps) {
  return (
    <div className="grid grid-cols-12 gap-x-5">
      <div className="px-2 py-6 sm:px-6 lg:col-span-3 lg:px-0 lg:py-0">
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
