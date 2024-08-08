import React from "react";
import { SettingsMenuItemProps } from "../components/ui/layouts/settings/SettingsMenuItem";
import { AssetNavbarMenuItem } from "../components/ui/layouts/authenticated/AuthenticatedLayoutNavbar";
import { PositionModel } from "@navtrack/shared/api/model/generated";

export type AppSlots = {
  assetNavbarMenuItems?: AssetNavbarMenuItem[];
  assetsSidebarTitle?: React.ReactNode;
  accountSettingsMenuItems?: SettingsMenuItemProps[];
  assetSettingsMenuItems?: SettingsMenuItemProps[];
  assetAddFooterBlock?: React.ReactNode;
  assetDeleteModalBlock?: React.ReactNode;
  externalLogin?: React.ReactNode;
  linkAccountWithExternalLoginPage?: React.ReactNode;
  captcha?: React.ReactNode;
  assetLiveTrackingPositionCardExtraItems?: (
    position: PositionModel
  ) => React.ReactNode;
  settingsPasswordAuthenticationBlock?: React.ReactNode;
};

export const SlotContext = React.createContext<AppSlots | undefined>({});
