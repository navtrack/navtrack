import { ReactNode, useContext, useMemo } from "react";
import { faCog, faHdd, faUsers } from "@fortawesome/free-solid-svg-icons";
import { generatePath } from "react-router-dom";
import { Paths } from "../../../../app/Paths";
import { useCurrentAsset } from "@navtrack/shared/hooks/assets/useCurrentAsset";
import { SettingsLayout } from "../../../ui/layouts/settings/SettingsLayout";
import { SettingsMenuItemProps } from "../../../ui/layouts/settings/SettingsMenuItem";
import { AuthenticatedLayoutTwoColumns } from "../../../ui/layouts/authenticated/AuthenticatedLayoutTwoColumns";
import { SlotContext } from "../../../../app/SlotContext";

type AssetSettingsLayoutProps = {
  children?: ReactNode;
};

export const defaultAssetSettingsMenuItems: SettingsMenuItemProps[] = [
  {
    label: "assets.settings.sidebar.settings",
    path: Paths.AssetsSettings,
    icon: faCog,
    priority: 100
  },
  {
    label: "assets.settings.sidebar.device",
    path: Paths.AssetsSettingsDevice,
    icon: faHdd,
    priority: 200
  },
  {
    label: "assets.settings.sidebar.access",
    path: Paths.AssetsSettingsAccess,
    icon: faUsers,
    priority: 300
  }
];

export function AssetSettingsLayout(props: AssetSettingsLayoutProps) {
  const currentAsset = useCurrentAsset();
  const slots = useContext(SlotContext);

  const menuItems: SettingsMenuItemProps[] = useMemo(
    () =>
      (slots?.assetSettingsMenuItems ?? defaultAssetSettingsMenuItems).map(
        (item) => ({
          ...item,
          path: generatePath(item.path, {
            id: currentAsset.data?.id ?? ""
          })
        })
      ),
    [currentAsset.data?.id, slots?.assetSettingsMenuItems]
  );

  return (
    <AuthenticatedLayoutTwoColumns>
      <div className="flex gap-x-6">
        <SettingsLayout menuItems={menuItems}>{props.children}</SettingsLayout>
      </div>
    </AuthenticatedLayoutTwoColumns>
  );
}
