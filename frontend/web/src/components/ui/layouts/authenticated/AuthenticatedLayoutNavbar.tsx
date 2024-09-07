import {
  faChartLine,
  faCog,
  faDatabase,
  faGauge,
  faHome,
  faMapMarkerAlt,
  faPlus,
  faRoute
} from "@fortawesome/free-solid-svg-icons";
import { Link, generatePath } from "react-router-dom";
import { Paths } from "../../../../app/Paths";
import { AuthenticatedLayoutNavbarProfile } from "./AuthenticatedLayoutNavbarProfile";
import { NavtrackLogoDark } from "../../logo/NavtrackLogoDark";
import { AuthenticatedLayoutNavbarItem } from "./AuthenticatedLayoutNavbarItem";
import { FormattedMessage } from "react-intl";
import { IconProp } from "@fortawesome/fontawesome-svg-core";
import { AssetRoleType } from "@navtrack/shared/api/model/generated";
import { Button } from "../../button/Button";
import { useRecoilValue } from "recoil";
import { currentAssetIdAtom } from "@navtrack/shared/state/assets";

type AuthenticatedLayoutNavbarProps = {
  hideLogo?: boolean;
};

export type AssetNavbarMenuItem = {
  label: string;
  path: string;
  icon: IconProp;
  priority: number;
  role?: AssetRoleType;
};

export const navbarMenuItems: AssetNavbarMenuItem[] = [
  {
    label: "generic.home",
    path: Paths.Home,
    icon: faHome,
    priority: 10
  },
  // {
  //   label: "generic.dashboard",
  //   path: Paths.AssetsDashboard,
  //   icon: faGauge,
  //   priority: 20
  // },
  {
    label: "generic.reports",
    path: Paths.Reports,
    icon: faChartLine,
    priority: 20
  },
  {
    label: "generic.settings",
    path: Paths.SettingsAccount,
    icon: faCog,
    priority: 30
  }
];

export const assetNavbarMenuitems: AssetNavbarMenuItem[] = [
  {
    label: "navbar.asset.live-tracking",
    path: Paths.AssetsLive,
    icon: faMapMarkerAlt,
    priority: 10,
    role: AssetRoleType.Viewer
  },
  {
    label: "generic.dashboard",
    path: Paths.AssetsDashboard,
    icon: faGauge,
    priority: 15,
    role: AssetRoleType.Viewer
  },
  {
    label: "navbar.asset.trips",
    path: Paths.AssetsTrips,
    icon: faRoute,
    priority: 20,
    role: AssetRoleType.Viewer
  },
  {
    label: "navbar.asset.log",
    path: Paths.AssetsLog,
    icon: faDatabase,
    priority: 30,
    role: AssetRoleType.Viewer
  },
  {
    label: "navbar.asset.settings",
    path: Paths.AssetsSettings,
    icon: faCog,
    priority: 40,
    role: AssetRoleType.Owner
  }
];

export function AuthenticatedLayoutNavbar(
  props: AuthenticatedLayoutNavbarProps
) {
  const currentAssetId = useRecoilValue(currentAssetIdAtom);

  const menuItems =
    currentAssetId !== undefined ? assetNavbarMenuitems : navbarMenuItems;

  return (
    <nav className="relative bg-white px-4 shadow">
      <div className="flex h-16 justify-between">
        <div className="flex">
          {!props.hideLogo && (
            <div className="flex h-16 w-64 items-center">
              <Link to={Paths.Home} className="flex items-center">
                <NavtrackLogoDark className="h-10 w-10 p-2" />
                <span className="ml-2 text-2xl font-semibold tracking-wide text-gray-900">
                  <FormattedMessage id="navtrack" />
                </span>
              </Link>
            </div>
          )}
          <div className="flex space-x-8">
            {menuItems.map((item) => (
              <AuthenticatedLayoutNavbarItem
                key={item.label}
                labelId={item.label}
                icon={item.icon}
                to={generatePath(item.path, {
                  id: `${currentAssetId}`
                })}
              />
            ))}
          </div>
        </div>
        <div className="flex items-center space-x-3">
          <Link to={Paths.AssetsAdd}>
            <Button size="md" color="secondary" icon={faPlus}>
              <FormattedMessage id="generic.new-asset" />
            </Button>
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
