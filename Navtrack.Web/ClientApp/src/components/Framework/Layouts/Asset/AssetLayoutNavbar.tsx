import React, { useContext } from "react";
import { Link, useRouteMatch, useLocation } from "react-router-dom";
import AppContext from "services/AppContext";
import Icon from "components/Framework/Util/Icon";
import classNames from "classnames";

export default function AssetLayoutNavbar() {
  const { appContext } = useContext(AppContext);
  const match = useRouteMatch<{ assetId?: string }>();
  const assetId = match.params.assetId ? parseInt(match.params.assetId) : 0;
  const asset = appContext.assets && appContext.assets.find(x => x.id === assetId);

  return (
    <>
      {asset && (
        <div className="bg-white shadow flex text-sm" style={{ minWidth: "630px" }}>
          <div className="mx-4 py-2 text-center font-semibold">
            <div className="pr-4 border-r">{asset.name}</div>
          </div>
          <ul className="flex flex-row py-2">
            <LinkItem url={`/live/${asset.id}`} icon="fa-map-marker-alt" text="Live Tracking" />
            <LinkItem url={`/log/${asset.id}`} icon="fa-database" text="Log" />
            <LinkItem url={`/reports/${asset.id}`} icon="fa-table" text="Reports" />
            <LinkItem url={`/alerts/${asset.id}`} icon="fa-bell" text="Alerts" />
            <LinkItem url={`/settings/${asset.id}`} icon="fa-cog" text="Settings" />
          </ul>
        </div>
      )}
    </>
  );
}

type Props = {
  url: string;
  icon: string;
  text: string;
};

function LinkItem(props: Props) {
  const location = useLocation();
  const isHighlighted = location.pathname.includes(props.url);

  return (
    <li className={classNames("text-gray-600 hover:text-gray-900 mr-4", { "text-gray-900": isHighlighted })}>
      <Link to={props.url}>
        <Icon className={props.icon} margin={1} /> {props.text}
      </Link>
    </li>
  );
}
