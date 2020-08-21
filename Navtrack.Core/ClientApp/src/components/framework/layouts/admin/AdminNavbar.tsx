import React, { useContext } from "react";
import { useHistory } from "react-router-dom";
import AppContext from "framework/appContext/AppContext";
import { AuthenticationService } from "framework/authentication/AuthenticationService";
import { FormattedMessage } from "react-intl";
import useClickOutside from "framework/hooks/useClickOutside";
import Button from "components/library/elements/Button";
import Icon from "components/library/util/Icon";

export default function AdminNavbar() {
  const { appContext } = useContext(AppContext);
  const history = useHistory();
  const [menuIsVisible, showMenu] = useClickOutside();

  const handleLogout = () => {
    AuthenticationService.clearAuthenticationInfo();

    history.push("/");
  };

  return (
    <nav className="p-3 flex bg-gray-900 shadow z-50">
      <h1 className="text-white font-medium">
        <FormattedMessage id="navbar.title" />
      </h1>
      <div className="flex-grow flex flex-row text-white justify-end items-center">
        <div className="mr-3">
          <Button color="primary" onClick={() => history.push("/assets/add")} size="xs">
            <FormattedMessage id="navbar.newAsset" />
          </Button>
        </div>
        <div className="mx-2 cursor-pointer">
          <div className="relative inline-block" onClick={(e) => showMenu(e)}>
            <i className="fa fa-user mr-1" />
            <span className="font-medium text-sm">
              {appContext.authenticationInfo && appContext.authenticationInfo.email}
            </span>
            {menuIsVisible && (
              <div className="mt-2 absolute right-0 fadeIn animated faster text-sm">
                <div className="w-48 bg-white rounded-lg shadow-lg overflow-hidden">
                  <div
                    className="block px-6 py-3 leading-tight hover:bg-gray-200 text-gray-600 hover:text-gray-900"
                    onClick={handleLogout}>
                    <Icon className="fa-sign-out-alt" /> <FormattedMessage id="navbar.logout" />
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
