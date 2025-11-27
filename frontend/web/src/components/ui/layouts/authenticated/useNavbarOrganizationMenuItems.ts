import {
  faChartLine,
  faCog,
  faGauge,
  faHdd,
  faMapMarkerAlt,
  faUser,
  faUsers
} from "@fortawesome/free-solid-svg-icons";
import { Paths } from "../../../../app/Paths";
import { useCurrentOrganization } from "@navtrack/shared/hooks/current/useCurrentOrganization";
import { useMemo } from "react";
import { OrganizationUserRole } from "@navtrack/shared/api/model";
import { NavbarMenuItem } from "./AuthenticatedLayoutNavbar";

export function useNavbarOrganizationMenuItems() {
  const currentOrganization = useCurrentOrganization();

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

  return organizationNavbarMenuItems;
}
