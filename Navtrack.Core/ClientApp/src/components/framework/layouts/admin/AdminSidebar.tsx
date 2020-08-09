import React, { useContext } from "react";
import { Link } from "react-router-dom";
import AppContext from "framework/appContext/AppContext";
import Icon from "components/library/util/Icon";
import Authorize from "components/library/authorization/Authorize";
import { UserRole } from "apis/types/user/UserRole";

export default function AdminSidebar() {
  const { appContext } = useContext(AppContext);

  return (
    <nav
      className="p-3 bg-white w-48 text-sm shadow z-10"
      style={{ width: "180px", minWidth: "180px" }}>
      <div className="mb-2 text-gray-600 uppercase tracking-wide font-semibold text-xs flex items-center">
        <div>Assets</div>
        <div className="flex-grow text-right"></div>
      </div>
      {appContext.assets ? (
        appContext.assets.length > 0 ? (
          <ul>
            {appContext.assets.map((x) => (
              <li key={x.id} className="mb-2">
                <Link className="text-gray-600 hover:text-gray-900" to={`/assets/${x.id}/live`}>
                  <Icon className="fa-circle w-4 text-green-600 text-xs" margin={1} /> {x.name}
                </Link>
              </li>
            ))}
          </ul>
        ) : (
          <div>No assets.</div>
        )
      ) : (
        <div>Loading assets.</div>
      )}
      <Authorize userRole={UserRole.Admin}>
        <hr className="my-3" />
        <h5 className="mb-2 text-gray-600 uppercase tracking-wide font-semibold text-xs">Admin</h5>
        <ul>
          <li className="mb-2">
            <Link className="text-gray-600 hover:text-gray-900" to="/admin/users">
              <Icon className="fa-user w-4" margin={1} /> Users
            </Link>
          </li>
        </ul>
      </Authorize>
    </nav>
  );
}
