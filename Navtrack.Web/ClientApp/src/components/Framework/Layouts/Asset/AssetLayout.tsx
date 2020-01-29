import React, { useContext } from "react";
import AdminLayout from "../Admin/AdminLayout";
import { Link, useRouteMatch } from "react-router-dom";
import AppContext from "services/AppContext";
import Icon from "components/Framework/Util/Icon";

type Props = {
  name: string,
  id: number,
  children: React.ReactNode
}

export default function AssetLayout(props: Props) {
  const { appContext } = useContext(AppContext);
  const match = useRouteMatch<{ assetId?: string }>();

  const assetId = match.params.assetId ? parseInt(match.params.assetId) : 0;
  const asset = appContext.assets && appContext.assets.find(x => x.id === assetId);

  return (
    <AdminLayout hidePadding={true}>
      {asset && <div className="bg-white shadow flex text-sm">
        <span className="mx-4 py-2 w-20 text-center font-semibold">{asset.name}</span>
        <ul className="flex flex-row py-2">
          <li className="text-gray-600 hover:text-gray-900 mr-4">
            <Link to={`/live/${asset.id}`}><Icon className="fa-map-marker-alt" margin={1} /> Live Tracking</Link>
          </li>
          <li className="text-gray-600 hover:text-gray-900 mx-4">
            <Link to={`/log/${asset.id}`}><Icon className="fa-database" margin={1} /> Log</Link>
          </li>
          <li className="text-gray-600 hover:text-gray-900 mx-4">
            <Link to={`/reports/${asset.id}`}><Icon className="fa-table" margin={1} /> Reports</Link>
          </li>
          <li className="text-gray-600 hover:text-gray-900 mx-4">
            <Link to={`/alerts/${asset.id}`}><Icon className="fa-bell" margin={1} /> Alerts</Link>
          </li>
          <li className="text-gray-600 hover:text-gray-900 mx-4">
            <Link to={`/settings/${asset.id}`}><Icon className="fa-cog" margin={1} /> Settings</Link>
          </li>
        </ul>
      </div>}
      <div className="pt-5 pl-5 pr-5 flex flex-col flex-grow">
        {props.children}
      </div>
    </AdminLayout>
  );
}