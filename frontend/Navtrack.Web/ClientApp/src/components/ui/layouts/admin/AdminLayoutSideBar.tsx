import Copyright from "../../../shared/Copyright";
import { FormattedMessage, useIntl } from "react-intl";
import { useHistory } from "react-router";
import NavtrackLogo from "../../../../assets/images/navtrack.svg";
import AdminLayoutSideBarAssets from "./AdminLayoutSideBarAssets";

export default function AdminLayoutSideBar() {
  const history = useHistory();
  const intl = useIntl();

  return (
    <div className="h-screen fixed w-64 flex flex-col">
      <div className="flex items-center h-14 flex-shrink-0 px-2 bg-gray-900">
        <div className="cursor-pointer flex items-center" onClick={() => history.push("/")}>
          <img
            src={NavtrackLogo}
            className="w-10 mr-1"
            alt={intl.formatMessage({ id: "navtrack" })}
          />
          <span className="text-white font-semibold text-2xl tracking-wide">
            <FormattedMessage id="navtrack" />
          </span>
        </div>
      </div>
      <div className="text-white p-4 text-xs font-medium tracking-wider uppercase bg-gray-800">
        <FormattedMessage id="sidebar.assets" />
      </div>
      <div
        className="flex flex-grow flex-col bg-gray-800 max-h-screen overflow-y-scroll py-2"
        style={{
          boxShadow:
            "inset 0 7px 9px -7px rgba(17,24,39,0.4), inset 0 -7px 9px -7px rgba(17,24,39,0.4)"
        }}>
        <AdminLayoutSideBarAssets />
      </div>
      <div className="text-white text-xs text-center p-4 bg-gray-800">
        <Copyright />
      </div>
    </div>
  );
}
