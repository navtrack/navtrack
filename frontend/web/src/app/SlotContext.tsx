import React from "react";
import { SettingsMenuItemProps } from "../components/ui/layouts/settings/SettingsMenuItem";
import { PositionDataModel } from "@navtrack/shared/api/model";

export type AppSlots = {
  accountSettingsMenuItems?: SettingsMenuItemProps[];
  organizationSettingsMenuItems?: SettingsMenuItemProps[];
  assetSettingsMenuItems?: SettingsMenuItemProps[];
  assetAddFooterBlock?: React.ReactNode;
  assetDeleteModalBlock?: React.ReactNode;
  externalLogin?: React.ReactNode;
  navbarAdditional?: React.ReactNode;
  linkAccountWithExternalLoginPage?: React.ReactNode;
  captcha?: React.ReactNode;
  assetLiveTrackingPositionCardExtraItems?: (
    position: PositionDataModel
  ) => React.ReactNode;
  settingsPasswordAuthenticationBlock?: React.ReactNode;
};

export const SlotContext = React.createContext<AppSlots | undefined>({});
