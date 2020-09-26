import classNames from "classnames";
import React, { useContext } from "react";
import { Link, useLocation } from "react-router-dom";
import { AppContext } from "../../services/appContext/AppContext";
import useAsset from "../../services/hooks/useAsset";
import Icon from "../shared/util/Icon";

export default function AssetLayoutNavbar() {
  const { appContext } = useContext(AppContext);
  const asset = useAsset();

  return (
    <div
      className="shadow flex text-sm z-30 relative"
      style={{
        minWidth: "630px",
        backdropFilter: appContext.mapIsVisible ? "blur(10px)" : "",
        backgroundColor: appContext.mapIsVisible
          ? "rgba(255, 255, 255, 0.9)"
          : "rgba(255, 255, 255, 0.6)"
      }}>
      <div className="ml-4 py-2 text-center font-semibold">
        <div className="pr-4 border-r">{asset?.name}</div>
      </div>
      <ul className="flex flex-row">
        <NavbarMenuItem url={`/assets/${asset?.id}/live`} icon="fa-map-marker-alt">
          Live Tracking
        </NavbarMenuItem>
        <NavbarMenuItem url={`/assets/${asset?.id}/locations`} icon="fa-database">
          Locations
        </NavbarMenuItem>
        <NavbarMenuItem url={`/assets/${asset?.id}/trips`} icon="fa-route">
          Trips
        </NavbarMenuItem>
        <NavbarMenuItem url={`/assets/${asset?.id}/reports`} icon="fa-table">
          Reports
        </NavbarMenuItem>
        <NavbarMenuItem url={`/assets/${asset?.id}/alerts`} icon="fa-bell">
          Alerts
        </NavbarMenuItem>
        <NavbarMenuItem url={`/assets/${asset?.id}/settings`} icon="fa-cog">
          Settings
        </NavbarMenuItem>
      </ul>
    </div>
  );
}

function NavbarMenuItem(props: { url: string; icon: string; children: string }) {
  const location = useLocation();
  const isHighlighted = location.pathname.includes(props.url);

  return (
    <Link to={props.url}>
      <li
        className={classNames(
          "text-gray-600 hover:text-gray-900 py-2 px-4 hover:border-gray-400 border-b-2 border-transparent font-medium",
          {
            "text-gray-900 border-b-2 border-orange-600 hover:border-orange-600": isHighlighted
          }
        )}>
        <Icon className={props.icon} margin={1} /> {props.children}
      </li>
    </Link>
  );
}
