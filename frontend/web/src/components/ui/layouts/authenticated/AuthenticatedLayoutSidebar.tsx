import { Copyright } from "../../../shared/Copyright";
import { FormattedMessage } from "react-intl";
import { Link } from "react-router-dom";
import { AuthenticatedLayoutSidebarAssets } from "./AuthenticatedLayoutSidebarAssets";
import { Paths } from "../../../../app/Paths";
import { NavtrackLogoDark } from "../../logo/NavtrackLogoDark";

export function AuthenticatedLayoutSidebar() {
  return (
    <div className="absolute bottom-0 top-0 flex w-64 flex-col">
      <div className="relative flex h-16 items-center bg-gray-900 px-4">
        <Link to={Paths.Home} className="flex items-center">
          <NavtrackLogoDark className="h-10 p-2" />
          <span className="ml-2 text-2xl font-semibold tracking-wide text-white">
            <FormattedMessage id="navtrack" />
          </span>
        </Link>
      </div>
      <div className="flex h-12 items-center bg-gray-800 px-4 text-xs font-medium uppercase tracking-wider text-white">
        <FormattedMessage id="sidebar.assets" />
      </div>
      <div
        className="relative flex-1 overflow-y-scroll bg-gray-800 py-2"
        style={{
          boxShadow:
            "inset 0 7px 9px -7px rgba(17,24,39,0.4), inset 0 -7px 9px -7px rgba(17,24,39,0.4)"
        }}>
        <AuthenticatedLayoutSidebarAssets />
      </div>
      <div className="flex h-12 items-center justify-center bg-gray-800 text-xs text-white">
        <Copyright />
      </div>
    </div>
  );
}
