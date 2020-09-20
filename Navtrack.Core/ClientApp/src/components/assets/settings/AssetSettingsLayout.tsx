import React, { useEffect, useState, useCallback } from "react";
import { useLocation, Switch, Route, useRouteMatch } from "react-router";
import { Link } from "react-router-dom";
import classNames from "classnames";
import { AssetApi } from "../../../apis/AssetApi";
import { AssetModel } from "../../../apis/types/asset/AssetModel";
import useAssetId from "../../../services/hooks/useAssetId";
import Icon from "../../shared/util/Icon";
import AssetSettingsDevice from "./AssetSettingsDevice";
import AssetSettingsGeneral from "./AssetSettingsGeneral";
import AssetSettingsDeviceLayout from "./device/AssetSettingsDeviceLayout";
import useMap from "../../../services/hooks/useMap";

export default function AssetSettingsLayout() {
  useMap(false);
  const assetId = useAssetId();
  const [asset, setAsset] = useState<AssetModel>();
  let { path } = useRouteMatch();

  const getAsset = useCallback(() => {
    AssetApi.get(assetId).then((x) => {
      setAsset(x);
    });
  }, [assetId]);

  useEffect(() => {
    getAsset();
  }, [getAsset]);

  return (
    <>
      {asset && (
        <div className="bg-white shadow p-3 rounded flex z-0">
          <div className="mr-3 border-r" style={{ width: "180px" }}>
            <ul>
              <MenuItem url={`/assets/${assetId}/settings`} icon="fa-cog">
                General
              </MenuItem>
              <MenuItem url={`/assets/${assetId}/settings/device`} icon="fa-hdd">
                Device
              </MenuItem>
            </ul>
          </div>
          <div className="w-full">
            <Switch>
              <Route exact path={path}>
                <AssetSettingsGeneral asset={asset} refreshAsset={getAsset} />
              </Route>
              <Route path={`${path}/device/:deviceId`}>
                <AssetSettingsDeviceLayout asset={asset} />
              </Route>
              <Route path={`${path}/device`}>
                <AssetSettingsDevice asset={asset} refreshAsset={getAsset} />
              </Route>
            </Switch>
          </div>
        </div>
      )}
    </>
  );
}

const MenuItem = (props: { children: string; url: string; icon: string }) => {
  const location = useLocation();
  const isHighlighted =
    (props.url.endsWith("settings") && location.pathname === props.url) ||
    (!props.url.endsWith("settings") && location.pathname.includes(props.url));

  return (
    <Link to={props.url}>
      <li
        className={classNames(
          "text-sm font-medium text-gray-600 hover:text-gray-900 hover:bg-gray-100 px-2 py-1 mb-1 mr-1 rounded",
          {
            "text-gray-800 bg-gray-200 hover:bg-gray-200": isHighlighted
          }
        )}>
        <Icon className={props.icon} margin={1} /> {props.children}
      </li>
    </Link>
  );
};
