import { NewButton } from "../../button/NewButton";
import { Icon } from "../../icon/Icon";
import {
  faCog,
  faDatabase,
  faMapMarkerAlt,
  faPlus,
  faRoute
} from "@fortawesome/free-solid-svg-icons";
import { Link, generatePath } from "react-router-dom";
import { Paths } from "../../../../app/Paths";
import { AuthenticatedLayoutNavbarProfile } from "./AuthenticatedLayoutNavbarProfile";
import { NavtrackLogoDark } from "../../logo/NavtrackLogoDark";
import { AuthenticatedLayoutNavbarItem } from "./AuthenticatedLayoutNavbarItem";
import { useCurrentAsset } from "@navtrack/shared/hooks/assets/useCurrentAsset";
import { FormattedMessage } from "react-intl";
import { useContext, useMemo } from "react";
import { SlotContext } from "../../../../app/SlotContext";
import { IconProp } from "@fortawesome/fontawesome-svg-core";
import { useAssetAuthorize } from "@navtrack/shared/hooks/assets/useAssetAuthorize";
import { AssetRoleType } from "@navtrack/shared/api/model/generated";

type AuthenticatedLayoutNavbarProps = {
  hideLogo?: boolean;
};

export type AssetNavbarMenuItem = {
  label: string;
  path: string;
  icon: IconProp;
  priority: number;
  role: AssetRoleType;
};

export const defaultAssetNavbarMenuItems: AssetNavbarMenuItem[] = [
  {
    label: "navbar.asset.live-tracking",
    path: Paths.AssetsLive,
    icon: faMapMarkerAlt,
    priority: 1,
    role: AssetRoleType.Viewer
  },
  {
    label: "navbar.asset.log",
    path: Paths.AssetsLog,
    icon: faDatabase,
    priority: 2,
    role: AssetRoleType.Viewer
  },
  {
    label: "navbar.asset.trips",
    path: Paths.AssetsTrips,
    icon: faRoute,
    priority: 3,
    role: AssetRoleType.Viewer
  },
  {
    label: "navbar.asset.settings",
    path: Paths.AssetsSettings,
    icon: faCog,
    priority: 4,
    role: AssetRoleType.Owner
  }
];

export function AuthenticatedLayoutNavbar(
  props: AuthenticatedLayoutNavbarProps
) {
  const slots = useContext(SlotContext);
  const currentAsset = useCurrentAsset();
  const assetAuthorize = useAssetAuthorize();

  const menuItmes = useMemo(
    () =>
      (slots?.assetNavbarMenuItems ?? defaultAssetNavbarMenuItems).filter(
        (x) =>
          currentAsset.data && assetAuthorize(currentAsset.data?.id, x.role)
      ),
    [assetAuthorize, currentAsset.data, slots?.assetNavbarMenuItems]
  );

  return (
    <nav className="relative bg-white px-4 shadow">
      <div className="flex h-16 justify-between">
        <div className="flex">
          {!props.hideLogo && (
            <div className="flex h-16 items-center">
              <Link to={Paths.Home} className="flex items-center">
                <NavtrackLogoDark className="h-10 p-2" />
                <span className="ml-2 text-2xl font-semibold tracking-wide text-gray-900">
                  <FormattedMessage id="navtrack" />
                </span>
              </Link>
            </div>
          )}
          <div className="flex space-x-8">
            {currentAsset.data?.id !== undefined && (
              <>
                {menuItmes.map((item) => (
                  <AuthenticatedLayoutNavbarItem
                    key={item.label}
                    labelId={item.label}
                    icon={item.icon}
                    to={generatePath(item.path, {
                      id: currentAsset.data?.id
                    })}
                  />
                ))}
              </>
            )}
          </div>
        </div>
        <div className="flex items-center space-x-3">
          <Link to={Paths.AssetsAdd}>
            <NewButton size="md" color="secondary">
              <div className="flex items-center justify-center">
                <Icon icon={faPlus} className="mr-2" size="sm" />
                <FormattedMessage id="generic.new-asset" />
              </div>
            </NewButton>
          </Link>
          {/* <button
            type="button"
            className="relative h-8 w-8 rounded-full bg-white p-1 text-gray-900 hover:text-gray-500 focus:outline-none focus:ring-2 focus:ring-blue-700 focus:ring-offset-2">
            <Icon icon={faBell} size="lg" />
          </button> */}
          <AuthenticatedLayoutNavbarProfile />
        </div>
      </div>
    </nav>
  );
}
