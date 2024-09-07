import React from "react";
import { SettingsMenuItemProps } from "../components/ui/layouts/settings/SettingsMenuItem";
import { MessagePositionModel } from "@navtrack/shared/api/model/generated";

export type AppSlots = {
  assetsSidebarTitle?: React.ReactNode;
  accountSettingsMenuItems?: SettingsMenuItemProps[];
  assetSettingsMenuItems?: SettingsMenuItemProps[];
  assetAddFooterBlock?: React.ReactNode;
  assetDeleteModalBlock?: React.ReactNode;
  externalLogin?: React.ReactNode;
  linkAccountWithExternalLoginPage?: React.ReactNode;
  captcha?: React.ReactNode;
  assetLiveTrackingPositionCardExtraItems?: (
    position: MessagePositionModel
  ) => React.ReactNode;
  settingsPasswordAuthenticationBlock?: React.ReactNode;
};

export const SlotContext = React.createContext<AppSlots | undefined>({});
