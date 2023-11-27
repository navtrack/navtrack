import { useContext } from "react";
import { SettingsLayout } from "../ui/layouts/settings/SettingsLayout";
import { faUser, faKey } from "@fortawesome/free-solid-svg-icons";
import { Paths } from "../../app/Paths";
import { SettingsMenuItemProps } from "../ui/layouts/settings/SettingsMenuItem";
import { SlotContext } from "../../app/SlotContext";
import { AuthenticatedLayoutOneColumn } from "../ui/layouts/authenticated/AuthenticatedLayoutOneColumn";

export const defaultAccountSettingsMenuItem: SettingsMenuItemProps[] = [
  {
    label: "settings.menu.account",
    path: Paths.SettingsAccount,
    icon: faUser,
    priority: 10
  },
  {
    label: "settings.menu.password",
    path: Paths.SettingsPassword,
    icon: faKey,
    priority: 20
  }
];

type AccountSettingsLayoutProps = {
  children?: React.ReactNode;
};

export function AccountSettingsLayout(props: AccountSettingsLayoutProps) {
  const slotContext = useContext(SlotContext);

  return (
    <AuthenticatedLayoutOneColumn>
      <div className="mx-auto flex max-w-7xl gap-x-6 p-8">
        <SettingsLayout
          menuItems={
            slotContext?.accountSettingsMenuItems ??
            defaultAccountSettingsMenuItem
          }>
          {props.children}
        </SettingsLayout>
      </div>
    </AuthenticatedLayoutOneColumn>
  );
}
