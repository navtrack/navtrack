import React from "react";
import { SettingsMenuItemProps } from "../components/ui/layouts/settings/SettingsMenuItem";
import { AssetNavbarMenuItem } from "../components/ui/layouts/authenticated/AuthenticatedLayoutNavbar";

export type AppSlots = {
  assetNavbarMenuItems?: AssetNavbarMenuItem[];
  accountSettingsMenuItems?: SettingsMenuItemProps[];
  assetSettingsMenuItems?: SettingsMenuItemProps[];
  assetAddFooterBlock?: React.ReactNode;
};

export const SlotContext = React.createContext<AppSlots | undefined>({});
