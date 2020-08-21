import React, { useState, useCallback, useEffect } from "react";
import { Route, Switch, useRouteMatch } from "react-router";
import { DeviceModel } from "../../apis/types/device/DeviceModel";
import DeviceLayoutNavbar from "./DeviceLayoutNavbar";
import useDeviceId from "../../framework/hooks/useDeviceId";
import { DeviceApi } from "../../apis/DeviceApi";
import DeviceInfo from "./DeviceInfo";
import DeviceLog from "./DeviceLog";

export default function DeviceLayout() {
  let { path } = useRouteMatch();
  const deviceId = useDeviceId();
  const [device, setDevice] = useState<DeviceModel>();

  const getAsset = useCallback(() => {
    DeviceApi.get(deviceId).then((x) => {
      setDevice(x);
    });
  }, [deviceId]);

  useEffect(() => {
    getAsset();
  }, [getAsset]);

  return (
    <>
      {device && (
        <>
          <DeviceLayoutNavbar device={device} />
          <div className="pt-5 pl-5 pr-5 flex flex-col flex-grow">
            <Switch>
              <Route exact path={path}>
                <DeviceInfo device={device} />
              </Route>
              <Route path={`${path}/log`}>
                <DeviceLog device={device} />
              </Route>
            </Switch>
          </div>
        </>
      )}
    </>
  );
}
