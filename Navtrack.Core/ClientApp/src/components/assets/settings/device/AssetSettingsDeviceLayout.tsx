import React, { useEffect, useState, useCallback } from "react";
import { useLocation, Switch, Route, useRouteMatch } from "react-router";
import { Link } from "react-router-dom";
import classNames from "classnames";
import Icon from "components/library/util/Icon";
import { AssetModel } from "apis/types/asset/AssetModel";
import useAssetId from "framework/hooks/useAssetId";
import AssetSettingsDeviceInfo from "./AssetSettingsDeviceInfo";
import useDeviceId from "framework/hooks/useDeviceId";
import AssetSettingsDeviceConnections from "./AssetSettingsDeviceConnections";
import { DeviceModel } from "apis/types/device/DeviceModel";
import { DeviceApi } from "apis/DeviceApi";

type Props = {
  asset: AssetModel;
};

export default function AssetSettingsDeviceLayout(props: Props) {
  const assetId = useAssetId();
  const deviceId = useDeviceId();

  let { path } = useRouteMatch();
  const [device, setDevice] = useState<DeviceModel>();

  const getDevice = useCallback(() => {
    DeviceApi.get(deviceId).then((x) => {
      setDevice(x);
    });
  }, [deviceId]);

  useEffect(() => {
    getDevice();
  }, [getDevice]);

  return (
    <>
      <div className="font-medium border-b">
        {device?.deviceType.displayName} ({device?.deviceId})
      </div>
      <div className="py-1 border-b flex">
        <MenuItem url={`/assets/${assetId}/settings/device/${deviceId}/info`} icon="fa-info-circle">
          Info
        </MenuItem>
        <MenuItem
          url={`/assets/${assetId}/settings/device/${deviceId}/connections`}
          icon="fa-network-wired">
          Connections
        </MenuItem>
      </div>
      <div className="mt-2">
        {device && (
          <Switch>
            <Route path={`${path}/info`}>
              <AssetSettingsDeviceInfo asset={props.asset} device={device} />
            </Route>
            <Route path={`${path}/connections`}>
              <AssetSettingsDeviceConnections asset={props.asset} />
            </Route>
          </Switch>
        )}
      </div>
    </>
  );
}

const MenuItem = (props: { children: string; url: string; icon: string }) => {
  const location = useLocation();
  const isHighlighted = location.pathname.includes(props.url);

  return (
    <Link to={props.url} className="mr-1">
      <div
        className={classNames(
          "text-sm font-medium text-gray-600 hover:text-gray-900 hover:bg-gray-100 px-4 py-1 rounded",
          {
            "text-gray-800 bg-gray-200 hover:bg-gray-200": isHighlighted
          }
        )}>
        <Icon className={props.icon} margin={1} /> {props.children}
      </div>
    </Link>
  );
};
