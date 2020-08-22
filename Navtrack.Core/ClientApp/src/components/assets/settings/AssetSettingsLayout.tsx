import React, { useEffect, useState, useCallback } from "react";
import { useLocation, Switch, Route, useRouteMatch } from "react-router";
import { Link } from "react-router-dom";
import classNames from "classnames";
import Icon from "components/library/util/Icon";
import { AssetModel } from "apis/types/asset/AssetModel";
import { AssetApi } from "apis/AssetApi";
import useAssetId from "framework/hooks/useAssetId";
import AssetLayout from "components/framework/layouts/asset/AssetLayout";
import AssetSettingsGeneral from "./AssetSettingsGeneral";
import AssetSettingsDevice from "./AssetSettingsDevice";

export default function AssetSettingsLayout() {
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
    <AssetLayout>
      {asset && (
        <div className="bg-white shadow p-3 rounded flex">
          <div className="mr-3 border-r" style={{ width: "180px" }}>
            <ul>
              <MenuItem url={`/assets/${assetId}/settings`} icon="a">
                General
              </MenuItem>
              <MenuItem url={`/assets/${assetId}/settings/device`} icon="hdd">
                Device
              </MenuItem>
            </ul>
          </div>
          <div className="w-full">
            <Switch>
              <Route exact path={path}>
                <AssetSettingsGeneral asset={asset} refreshAsset={getAsset} />
              </Route>
              <Route path={`${path}/device`}>
                <AssetSettingsDevice asset={asset} refreshAsset={getAsset} />
              </Route>
            </Switch>
          </div>
        </div>
      )}
    </AssetLayout>
  );
}

const MenuItem = (props: { children: string; url: string; icon: string }) => {
  const location = useLocation();
  const isHighlighted = location.pathname === props.url;

  return (
    <Link to={props.url}>
      <li
        className={classNames(
          "font-medium text-gray-600 hover:text-gray-900 hover:bg-gray-100 px-2 py-1 m-1 rounded",
          {
            "text-gray-800 bg-gray-200 hover:bg-gray-200": isHighlighted
          }
        )}>
        <Icon className={props.icon} margin={1} /> {props.children}
      </li>
    </Link>
  );
};
