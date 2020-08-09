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

export default function AssetSettings() {
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
        <div className="bg-white shadow p-3 rounded mb-3 flex ">
          <div className="mr-3 border-r" style={{ width: "150px" }}>
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
          "text-gray-600 hover:text-gray-900 hover:border-gray-400 px-2 py-1 border-r-2 border-transparent",
          {
            "text-gray-900 border-orange-600 hover:border-orange-600": isHighlighted
          }
        )}>
        <Icon className={props.icon} margin={1} /> {props.children}
      </li>
    </Link>
  );
};
