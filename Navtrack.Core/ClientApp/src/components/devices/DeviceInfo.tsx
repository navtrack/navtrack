import React from "react";
import { DeviceModel } from "../../apis/types/device/DeviceModel";

type Props = {
  device: DeviceModel;
};

export default function DeviceInfo(props: Props) {
  return (
    <div className="bg-white shadow p-3 rounded mb-3 flex text-sm">
      <div className="w-1/2 pr-4">
        <div className="text-xl mb-1 pb-2 border-b">Device</div>
        <div className="pt-2 pb-4">
          <table className="border w-full">
            <tbody>
              <tr>
                <td
                  className="p-2 bg-gray-100 text-xs text-gray-700 font-medium border"
                  style={{ width: "120px" }}>
                  Device ID
                </td>
                <td className="p-2 font-medium border">{props.device.deviceId}</td>
              </tr>
              <tr>
                <td className="p-2 bg-gray-100 text-xs text-gray-700 font-medium border">
                  Manufacturer
                </td>
                <td className="p-2 font-medium border">{props.device.deviceType.manufacturer}</td>
              </tr>
              <tr>
                <td className="p-2 bg-gray-100 text-xs text-gray-700 font-medium border">Model</td>
                <td className="p-2 font-medium border">{props.device.deviceType.model}</td>
              </tr>
            </tbody>
          </table>
        </div>
      </div>
      <div className="w-1/2 pl-4">
        <div className="text-xl mb-1 pb-2 border-b">Statistics</div>
        <div className="pt-2 pb-4">
          <table className="border w-full">
            <tbody>
              <tr>
                <td
                  className="p-2 bg-gray-100 text-xs text-gray-700 font-medium border"
                  style={{ width: "120px" }}>
                  Locations
                </td>
                <td className="p-2 font-medium border">{props.device.locationsCount}</td>
              </tr>
            </tbody>
          </table>
        </div>
      </div>
    </div>
  );
}
