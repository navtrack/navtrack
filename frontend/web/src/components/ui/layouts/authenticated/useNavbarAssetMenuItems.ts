import {
  faBusinessTime,
  faChartLine,
  faClock,
  faCog,
  faDatabase,
  faGasPump,
  faGauge,
  faMapMarkerAlt,
  faRoute,
  faTachometer
} from "@fortawesome/free-solid-svg-icons";
import { Paths } from "../../../../app/Paths";
import { AssetUserRole } from "@navtrack/shared/api/model";
import { faStopCircle } from "@fortawesome/free-regular-svg-icons";
import { NavbarMenuItem } from "./AuthenticatedLayoutNavbar";

export function useNavbarAssetMenuItems() {
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
      label: "generic.trips",
      path: Paths.AssetTrips,
      icon: faRoute,
      order: 20
    },
    {
      label: "navbar.asset.reports",
      path: "",
      icon: faChartLine,
      order: 25,
      subMenuItems: [
        {
          label: "generic.distance",
          path: Paths.AssetReportsDistance,
          icon: faTachometer,
          order: 10
        },
        {
          label: "generic.fuel-consumption",
          path: Paths.AssetReportsFuelConsumption,
          icon: faGasPump,
          order: 20
        },
        {
          label: "generic.trips",
          path: Paths.AssetReportsTrips,
          icon: faRoute,
          order: 30
        },
        {
          label: "generic.stops",
          path: Paths.AssetReportsStops,
          icon: faStopCircle,
          order: 40
        },
        {
          label: "generic.working-hours",
          path: Paths.AssetReportsWorkingHours,
          icon: faClock,
          order: 50
        }
      ]
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

  return assetNavbarMenuitems;
}
