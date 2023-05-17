import {
  faCog,
  faDatabase,
  faMapMarkerAlt,
  faRoute
} from "@fortawesome/free-solid-svg-icons";
import { AdminLayoutNavBarProfile } from "./AdminLayoutNavBarProfile";
import { AdminLayoutNavBarItem } from "./AdminLayoutNavBarItem";
import { Button } from "../../shared/button/Button";
import { useHistory } from "react-router-dom";
import { FormattedMessage } from "react-intl";
import { useCurrentAsset } from "@navtrack/shared/hooks/assets/useCurrentAsset";

export function AdminLayoutNavBar() {
  const currentAsset = useCurrentAsset();
  const history = useHistory();

  return (
    <div className="z-10 flex h-14 bg-white shadow">
      <div className="ml-4 flex flex-1 space-x-8">
        {currentAsset && (
          <>
            <AdminLayoutNavBarItem
              labelId="navbar.asset.live-tracking"
              icon={faMapMarkerAlt}
              href={`/assets/${currentAsset.data?.id}/live`}
            />
            <AdminLayoutNavBarItem
              labelId="navbar.asset.log"
              icon={faDatabase}
              href={`/assets/${currentAsset.data?.id}/log`}
            />
            <AdminLayoutNavBarItem
              labelId="navbar.asset.trips"
              icon={faRoute}
              href={`/assets/${currentAsset.data?.id}/trips`}
            />
            {/* <AdminLayoutNavBarItem
              labelId="navbar.asset.reports"
              icon={faTable}
              href={`/assets/${id}/reports`}
            />
            <AdminLayoutNavBarItem
              labelId="navbar.asset.alerts"
              icon={faBell}
              href={`/assets/${id}/alerts`}
            /> */}
            <AdminLayoutNavBarItem
              labelId="navbar.asset.settings"
              icon={faCog}
              href={`/assets/${currentAsset.data?.id}/settings`}
            />
          </>
        )}
      </div>
      <div className="mr-4 flex items-center">
        <a className="mr-4" href="/assets/add">
          <Button
            onClick={(e) => {
              e.preventDefault();
              history.push("/assets/add");
            }}>
            <FormattedMessage id="navbar.new-asset" />
          </Button>
        </a>
        <AdminLayoutNavBarProfile />
      </div>
    </div>
  );
}
