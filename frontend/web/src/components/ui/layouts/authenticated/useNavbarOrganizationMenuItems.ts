import {
  faChartLine,
  faCog,
  faGasPump,
  faGauge,
  faHdd,
  faMapMarkerAlt,
  faTachometer,
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
        label: "dashboard",
        path: Paths.OrganizationDashboard,
        icon: faGauge,
        order: 20
      },
      {
        label: "navbar.asset.reports",
        path: "",
        icon: faChartLine,
        order: 25,
        subMenuItems: [
          {
            label: "distance",
            path: Paths.OrganizationReportsDistance,
            icon: faTachometer,
            order: 10
          },
          {
            label: "fuel-consumption",
            path: Paths.OrganizationReportsFuelConsumption,
            icon: faGasPump,
            order: 20
          }
        ]
      },
      {
        label: "assets",
        path: Paths.OrganizationAssets,
        icon: faHdd,
        order: 25,
        count: currentOrganization.data?.assetsCount
      },
      {
        label: "teams",
        path: Paths.OrganizationTeams,
        icon: faUsers,
        order: 30,
        count: currentOrganization.data?.teamsCount
      },
      {
        label: "users",
        path: Paths.OrganizationUsers,
        icon: faUser,
        order: 35,
        count: currentOrganization.data?.usersCount,
        organizationRole: OrganizationUserRole.Owner
      },
      {
        label: "settings",
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
