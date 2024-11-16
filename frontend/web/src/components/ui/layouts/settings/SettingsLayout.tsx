import { useMemo } from "react";
import { SettingsMenuItem, SettingsMenuItemProps } from "./SettingsMenuItem";

type SettingsLayoutProps = {
  menuItems: SettingsMenuItemProps[];
  children?: React.ReactNode;
};

export function SettingsLayout(props: SettingsLayoutProps) {
  const sortedMenuItems: SettingsMenuItemProps[] = useMemo(
    () => props.menuItems.sort((a, b) => a.priority - b.priority),
    [props.menuItems]
  );

  return (
    <>
      <div className="block w-56">
        <div className="flex-none space-y-1">
          {sortedMenuItems.map((item) => (
            <SettingsMenuItem {...item} key={item.path} />
          ))}
        </div>
      </div>
      <div className="flex-1 space-y-5">{props.children}</div>
    </>
  );
}
