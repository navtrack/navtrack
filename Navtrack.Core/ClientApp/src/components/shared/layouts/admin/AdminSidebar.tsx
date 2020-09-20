import React, { useContext } from "react";
import { Link } from "react-router-dom";
import { UserRole } from "../../../../apis/types/user/UserRole";
import { AppContext } from "../../../../services/appContext/AppContext";
import Authorize from "../../../shared/authorization/Authorize";
import Icon from "../../../shared/util/Icon";

export default function AdminSidebar() {
  const { appContext } = useContext(AppContext);

  return (
    <nav
      className="p-3 w-48 text-sm shadow-md z-40"
      style={{
        width: "180px",
        minWidth: "180px",
        backdropFilter: appContext.mapIsVisible ? "blur(10px)" : "",
        backgroundColor: appContext.mapIsVisible
          ? "rgba(255, 255, 255, 0.9)"
          : "rgba(255, 255, 255, 0.6)"
      }}>
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
