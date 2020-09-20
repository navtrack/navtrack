import React from "react";
import { DeviceModel } from "../../apis/types/device/DeviceModel";
import { useLocation } from "react-router";
import { Link } from "react-router-dom";
import Icon from "../shared/util/Icon";
import classNames from "classnames";

type Props = {
  device: DeviceModel;
};

export default function DeviceLayoutNavbar(props: Props) {
  return (
    <>
      {props.device && (
        <div className="bg-white shadow flex text-sm" style={{ minWidth: "630px" }}>
          <div className="ml-4 py-2 text-center font-normal flex">
            <div className="pr-2 text-blue-700 font-medium">
              <Link to={`/assets/${props.device.assetId}/live`}>BN01SBU</Link>
            </div>
            <div className="pr-2 font-light">/</div>
            <div className="pr-2 font-normal text-blue-700 font-medium">
              <Link to={`/assets/${props.device.assetId}/settings/device`}>Device</Link>
            </div>
            <div className="pr-2 font-light">/</div>
            <div className="pr-2 font-semibold">{props.device.deviceId}</div>
            <div className="pr-4 border-r font-normal">({props.device.deviceType.displayName})</div>
          </div>
          <ul className="flex flex-row">
            <LinkItem url={`/devices/${props.device.id}`} icon="fa-hdd">
              Info
            </LinkItem>
            {/* <LinkItem url={`/devices/${props.device.id}/log`} icon="fa-database">
              Log
            </LinkItem> */}
          </ul>
        </div>
      )}
    </>
  );
}

type LinkItemProps = {
  url: string;
  icon: string;
  children: string;
};

function LinkItem(props: LinkItemProps) {
  const location = useLocation();
  const isHighlighted = location.pathname === props.url;

  return (
    <Link to={props.url}>
      <li
        className={classNames(
          "text-gray-600 hover:text-gray-900 py-2 px-4 hover:border-gray-400 border-b-2 border-transparent font-medium",
          {
            "text-gray-900 border-b-2 border-orange-600 hover:border-orange-600": isHighlighted
          }
        )}>
        <Icon className={props.icon} margin={1} /> {props.children}
      </li>
    </Link>
  );
}
