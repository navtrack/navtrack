import React, { useContext } from "react";
import { useHistory } from "react-router-dom";
import AppContext from "services/AppContext";
import Icon from "components/Framework/Util/Icon";
import useClickOutside from "./useClickOutside";
import { AuthenticationService } from "services/Authentication/AuthenticationService";

export default function AdminNavbar() {
  const { appContext } = useContext(AppContext);
  const history = useHistory();
  const [menuIsVisible, showMenu] = useClickOutside();

  const handleLogout = () => {
    AuthenticationService.clearAuthenticationInfo();

    history.push("/");
  };

  return (
    <nav className="p-3 flex bg-gray-900 shadow z-20">
      <h1 className="text-white font-medium">NAVTRACK</h1>
      <div className="flex-grow flex flex-row text-white justify-end">
        <div className="mx-2 cursor-pointer">
          <div className="relative inline-block" onClick={e => showMenu(e)}>
            <i className="fa fa-user mr-1" />
            <span className="font-medium text-sm">{appContext.user && appContext.user.email}</span>
            {menuIsVisible && (
              <div className="mt-2 absolute right-0 fadeIn animated faster text-sm">
                <div className="w-48 bg-white rounded-lg shadow-lg overflow-hidden">
                  <div
                    className="block px-6 py-3 leading-tight hover:bg-gray-200 text-gray-600 hover:text-gray-900"
                    onClick={handleLogout}>
                    <Icon className="fa-sign-out-alt" /> Logout
                  </div>
                </div>
              </div>
            )}
          </div>
        </div>
      </div>
    </nav>
  );
}
