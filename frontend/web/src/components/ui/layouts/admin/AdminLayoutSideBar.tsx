import { Copyright } from "../../../shared/Copyright";
import { FormattedMessage } from "react-intl";
import { useHistory } from "react-router-dom";
import { AdminLayoutSideBarAssets } from "./AdminLayoutSideBarAssets";
import { NavtrackLogo } from "../../NavtrackLogo";

export function AdminLayoutSideBar() {
  const history = useHistory();

  return (
    <div className="fixed flex h-screen w-64 flex-col">
      <div className="flex h-14 flex-shrink-0 items-center bg-gray-900 px-2">
        <div
          className="flex cursor-pointer items-center"
          onClick={() => history.push("/")}>
          <div className="mr-1 w-10">
            <NavtrackLogo />
          </div>
          <span className="text-2xl font-semibold tracking-wide text-white">
            <FormattedMessage id="navtrack" />
          </span>
        </div>
      </div>
      <div className="bg-gray-800 p-4 text-xs font-medium uppercase tracking-wider text-white">
        <FormattedMessage id="sidebar.assets" />
      </div>
      <div
        className="flex max-h-screen flex-grow flex-col overflow-y-scroll bg-gray-800 py-2"
        style={{
          boxShadow:
            "inset 0 7px 9px -7px rgba(17,24,39,0.4), inset 0 -7px 9px -7px rgba(17,24,39,0.4)"
        }}>
        <AdminLayoutSideBarAssets />
      </div>
      <div className="bg-gray-800 p-4 text-center text-xs text-white">
        <Copyright />
      </div>
    </div>
  );
}
