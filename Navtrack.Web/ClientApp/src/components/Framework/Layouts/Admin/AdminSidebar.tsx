import React, { useContext } from "react";
import { Link } from "react-router-dom";
import AppContext from "services/AppContext";
import Icon from "components/Framework/Util/Icon";

export default function AdminSidebar() {
  const { appContext } = useContext(AppContext);

  return (
    <nav className="p-3 bg-white w-48 text-sm shadow z-10">
      <h5 className="mb-2 text-gray-500 uppercase tracking-wide font-semibold text-xs">Assets</h5>
      {appContext.assets ? appContext.assets.length > 0 ?
        <ul>
          {appContext.assets.map(x =>
            <li key={x.id} className="mb-2">
              <Link className="text-gray-600 hover:text-gray-900" to={`/live/${x.id}`}>
                <Icon className="fa-circle w-4 text-green-600 text-xs" margin={1} /> {x.name}
              </Link>
            </li>)}
        </ul>
        :
        <div>No assets.</div>
        :
        <div>Loading assets.</div>
      }
      <hr className="my-3" />
      <h5 className="mb-2 text-gray-500 uppercase tracking-wide font-semibold text-xs">Settings</h5>
      <ul>
        <li className="mb-2">
          <Link className="text-gray-600 hover:text-gray-900" to="/assets">
            <Icon className="fa-car-alt w-4" margin={1} /> Assets
                    </Link>
        </li>
        <li className="mb-2">
          <Link className="text-gray-600 hover:text-gray-900" to="/devices">
            <Icon className="fa-hdd w-4" margin={1} /> Devices
                    </Link>
        </li>
      </ul>
      {/* <h5 className="mb-2 text-gray-500 uppercase tracking-wide font-semibold text-xs">Admin</h5>
      <ul>
        <li className="mb-2">
          <Link className="text-gray-600 hover:text-gray-900" to="/users">
            <Icon className="fa-user w-4" margin={1} /> Users
                    </Link>
        </li>
      </ul> */}
    </nav>
  );
}