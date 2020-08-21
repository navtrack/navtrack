import React, { useContext } from "react";
import { Link, useRouteMatch, useLocation } from "react-router-dom";
import AppContext from "framework/appContext/AppContext";
import classNames from "classnames";
import Icon from "components/library/util/Icon";

export default function AssetLayoutNavbar() {
  const { appContext } = useContext(AppContext);
  const match = useRouteMatch<{ assetId?: string }>();
  const assetId = match.params.assetId ? parseInt(match.params.assetId) : 0;
  const asset = appContext.assets && appContext.assets.find((x) => x.id === assetId);

  return (
    <>
      {asset && (
        <div className="bg-white shadow flex text-sm z-30" style={{ minWidth: "630px" }}>
          <div className="ml-4 py-2 text-center font-semibold">
            <div className="pr-4 border-r">{asset.name}</div>
          </div>
          <ul className="flex flex-row">
            <LinkItem url={`/assets/${asset.id}/live`} icon="fa-map-marker-alt">
              Live Tracking
            </LinkItem>
            <LinkItem url={`/assets/${asset.id}/log`} icon="fa-database">
              Log
            </LinkItem>
            <LinkItem url={`/assets/${asset.id}/reports`} icon="fa-table">
              Reports
            </LinkItem>
            <LinkItem url={`/assets/${asset.id}/alerts`} icon="fa-bell">
              Alerts
            </LinkItem>
            <LinkItem url={`/assets/${asset.id}/settings`} icon="fa-cog">
              Settings
            </LinkItem>
          </ul>
        </div>
      )}
    </>
  );
}

type Props = {
  url: string;
  icon: string;
  children: string;
};

function LinkItem(props: Props) {
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
