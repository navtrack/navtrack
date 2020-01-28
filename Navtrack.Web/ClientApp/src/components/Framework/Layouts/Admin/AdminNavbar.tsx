import React, { useContext, useState, useEffect } from "react";
import { useHistory } from "react-router-dom";
import { AccountApi } from "services/Api/AccountApi";
import AppContext from "services/AppContext";
import Icon from "components/Framework/Util/Icon";
import { DefaultUserModel } from "services/Api/Model/UserModel";

export default function AdminNavbar() {
  const { appContext, setAppContext } = useContext(AppContext);
  const history = useHistory();
  const [show, setShow] = useState(false);

  const handleLogout = () => {
    AccountApi.logout().then(x => {
      if (x.ok) {
        setAppContext({ ...appContext, user: DefaultUserModel, authenticated: false });
        history.push("/");
      }
    })
  }

  useEffect(() => {
    function handleClickOutside() {
      setShow(false);
    }

    document.addEventListener("click", handleClickOutside);

    return () => {
      document.removeEventListener("click", handleClickOutside);
    }
  });

  return (
    <nav className="p-3 flex bg-gray-900 shadow z-20">
      <h1 className="text-white font-medium">NAVTRACK</h1>
      <div className="flex-grow flex flex-row text-white justify-end">
        <div className="mx-2 cursor-pointer">
          <div id="dropdown" className="relative inline-block" onClick={(e) => {
            e.stopPropagation();
            e.nativeEvent.stopImmediatePropagation();
            setShow(true);
          }}>
            <i className="fa fa-user mr-1" />
            <span className="font-medium text-sm">{appContext.user && appContext.user.email}</span>
            {show &&
              <div className="mt-2 absolute right-0 fadeIn animated faster">
                <div className="w-48 bg-white rounded-lg shadow-lg overflow-hidden">
                  <div className="block px-6 py-3 leading-tight hover:bg-gray-200 text-gray-600 hover:text-gray-900" onClick={handleLogout}>
                    <Icon icon="fa-sign-out-alt" /> Logout
                  </div>
                </div>
              </div>}
          </div>
        </div>
      </div>
    </nav>
  );
}