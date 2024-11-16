import React from "react";
import { SettingsMenuItemProps } from "../components/ui/layouts/settings/SettingsMenuItem";
import { MessagePosition } from "@navtrack/shared/api/model/generated";

export type AppSlots = {
  accountSettingsMenuItems?: SettingsMenuItemProps[];
  organizationSettingsMenuItems?: SettingsMenuItemProps[];
  assetSettingsMenuItems?: SettingsMenuItemProps[];
  assetAddFooterBlock?: React.ReactNode;
  assetDeleteModalBlock?: React.ReactNode;
  externalLogin?: React.ReactNode;
  linkAccountWithExternalLoginPage?: React.ReactNode;
  captcha?: React.ReactNode;
  assetLiveTrackingPositionCardExtraItems?: (
    position: MessagePosition
  ) => React.ReactNode;
  settingsPasswordAuthenticationBlock?: React.ReactNode;
};

export const SlotContext = React.createContext<AppSlots | undefined>({});
