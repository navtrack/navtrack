import {
  faChartLine,
  faCog,
  faDatabase,
  faGauge,
  faHdd,
  faMapMarkerAlt,
  faRoute,
  faUser,
  faUsers
} from "@fortawesome/free-solid-svg-icons";
import { NavLink, generatePath } from "react-router-dom";
import { Paths } from "../../../../app/Paths";
import { AuthenticatedLayoutNavbarProfile } from "./AuthenticatedLayoutNavbarProfile";
import { NavtrackLogoDark } from "../../logo/NavtrackLogoDark";
import { AuthenticatedLayoutNavbarItem } from "./AuthenticatedLayoutNavbarItem";
import { FormattedMessage } from "react-intl";
import { IconProp } from "@fortawesome/fontawesome-svg-core";
import { NavbarOrganization } from "./NavbarOrganization";
import { useCurrentOrganization } from "@navtrack/shared/hooks/current/useCurrentOrganization";
import { useCallback, useContext, useMemo } from "react";
import { useCurrentAsset } from "@navtrack/shared/hooks/current/useCurrentAsset";
import { c, classNames } from "@navtrack/shared/utils/tailwind";
import { NavbarBreadcrumbs } from "./NavbarBreadcrumbs";
import {
  AssetUserRole,
  OrganizationUserRole
} from "@navtrack/shared/api/model/generated";
import { SlotContext } from "../../../../app/SlotContext";
import { useAuthorize } from "@navtrack/shared/hooks/current/useAuthorize";

type AuthenticatedLayoutNavbarProps = {
  hideLogo?: boolean;
};

export type NavbarMenuItem = {
  label: string;
  path: string;
  icon: IconProp;
  order: number;
  count?: number;
  organizationRole?: OrganizationUserRole;
  assetRole?: AssetUserRole;
};

const assetNavbarMenuitems: NavbarMenuItem[] = [
  {
    label: "navbar.asset.live-tracking",
    path: Paths.AssetLive,
    icon: faMapMarkerAlt,
    order: 10
  },
  {
    label: "generic.dashboard",
    path: Paths.AssetDashboard,
    icon: faGauge,
    order: 15
  },
  {
    label: "navbar.asset.trips",
    path: Paths.AssetTrips,
    icon: faRoute,
    order: 20
  },
  {
    label: "navbar.asset.log",
    path: Paths.AssetLog,
    icon: faDatabase,
    order: 30
  },
  {
    label: "navbar.asset.settings",
    path: Paths.AssetSettings,
    icon: faCog,
    order: 40,
    assetRole: AssetUserRole.Owner
  }
];

export function AuthenticatedLayoutNavbar(
  props: AuthenticatedLayoutNavbarProps
) {
  const currentAsset = useCurrentAsset();
  const currentOrganization = useCurrentOrganization();
  const slots = useContext(SlotContext);
  const authorize = useAuthorize();

  const organizationNavbarMenuItems: NavbarMenuItem[] = useMemo(
    () => [
      {
        label: "navbar.asset.live-tracking",
        path: Paths.OrganizationLive,
        icon: faMapMarkerAlt,
        order: 10
      },
      {
        label: "generic.dashboard",
        path: Paths.OrganizationDashboard,
        icon: faGauge,
        order: 20
      },
      {
        label: "generic.reports",
        path: Paths.OrganizationReports,
        icon: faChartLine,
        order: 20
      },
      {
        label: "generic.assets",
        path: Paths.OrganizationAssets,
        icon: faHdd,
        order: 25,
        count: currentOrganization.data?.assetsCount
      },
      {
        label: "generic.teams",
        path: Paths.OrganizationTeams,
        icon: faUsers,
        order: 30,
        count: currentOrganization.data?.teamsCount
      },
      {
        label: "generic.users",
        path: Paths.OrganizationUsers,
        icon: faUser,
        order: 35,
        count: currentOrganization.data?.usersCount,
        organizationRole: OrganizationUserRole.Owner
      },
      {
        label: "generic.settings",
        path: Paths.OrganizationSettings,
        icon: faCog,
        order: 40,
        organizationRole: OrganizationUserRole.Owner
      }
    ],
    [
      currentOrganization.data?.assetsCount,
      currentOrganization.data?.teamsCount,
      currentOrganization.data?.usersCount
    ]
  );

  const menuItems = useMemo(
    () =>
      currentAsset.id !== undefined
        ? assetNavbarMenuitems.filter(
            (item) =>
              item.assetRole === undefined || authorize.asset(item.assetRole)
          )
        : currentOrganization.id !== undefined
        ? organizationNavbarMenuItems.filter(
            (item) =>
              item.organizationRole === undefined ||
              authorize.organization(item.organizationRole)
          )
        : [],
    [
      authorize,
      currentAsset.id,
      currentOrganization.id,
      organizationNavbarMenuItems
    ]
  );

  const getPath = useCallback(
    (path: string) => {
      const isAsset = path.includes("/assets/:id");
      const isOrganization = path.includes("/orgs/:id");

      return generatePath(path, {
        id: `${
          isAsset
            ? currentAsset.id
            : isOrganization
            ? currentOrganization.id
            : undefined
        }`
      });
    },
    [currentAsset.id, currentOrganization.id]
  );

  return (
    <div className="relative bg-white px-6 shadow">
      <div
        className={classNames(
          "flex h-14 flex-1 justify-between",
          c(menuItems.length > 0, "border-b border-gray-200")
        )}>
        <div className="flex items-center space-x-2 text-sm">
          {!props.hideLogo && (
            <div className="-ml-2 flex h-14 w-64 items-center">
              <NavLink to={Paths.Home} className="flex items-center">
                <NavtrackLogoDark className="h-10 w-10 p-2" />
                <span className="ml-2 text-2xl font-semibold tracking-wide text-gray-900">
                  <FormattedMessage id="navtrack" />
                </span>
              </NavLink>
            </div>
          )}
          <NavbarBreadcrumbs />
        </div>
        <div className="flex items-center space-x-4">
          {slots?.navbarAdditional}
          <NavbarOrganization />
          {/*<button
            type="button"
            className="relative h-8 w-8 rounded-full bg-white p-1 text-gray-900 hover:text-gray-500 focus:outline-none focus:ring-2 focus:ring-blue-700 focus:ring-offset-2">
            <Icon icon={faBell} size="lg" />
          </button> */}
          <AuthenticatedLayoutNavbarProfile />
        </div>
      </div>
      {menuItems.length > 0 && (
        <div className="flex h-14 space-x-8">
          {menuItems.map((item) => (
            <AuthenticatedLayoutNavbarItem
              key={item.label}
              labelId={item.label}
              icon={item.icon}
              to={getPath(item.path)}
              count={item.count}
            />
          ))}
        </div>
      )}
    </div>
  );
}
