import React from "react";
import classNames from "classnames";
import { DeviceTypeModel } from "../../apis/types/device/DeviceTypeModel";

type Props = {
  deviceType?: DeviceTypeModel;
  className?: string;
};

export default function DeviceConfiguration(props: Props) {
  return (
    <div className={classNames("text-sm", props.className)}>
      <h1 className="font-semibold mb-2">Device configuration</h1>
      <div className="mb-2">
        Use the parameters below to configure your device.
        <br />
        If the device supports hostnames, please use the hostname instead of the IP address.
      </div>
      <table className="w-full text-sm rounded border">
        <tbody>
          <tr>
            <td
              className="p-2 bg-gray-100 text-xs text-gray-700 font-medium border"
              style={{ width: "120px" }}>
              Hostname
            </td>
            <td className="p-2 font-medium border">a.navtrack.io</td>
          </tr>
          <tr>
            <td className="p-2 bg-gray-100 text-xs text-gray-700 font-medium border">IP Address</td>
            <td className="p-2 font-medium border">116.202.177.156</td>
          </tr>
          <tr>
            <td className="p-2 bg-gray-100 text-xs text-gray-700 font-medium border">Port</td>
            <td className="p-2 font-medium border">
              {props.deviceType ? (
                props.deviceType.protocol.port
              ) : (
                <>Choose a device type to see the port.</>
              )}
            </td>
          </tr>
        </tbody>
      </table>
    </div>
  );
}
