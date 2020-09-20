import React, { useState, useEffect } from "react";
import { DeviceApi } from "../../../../apis/DeviceApi";
import { AssetModel } from "../../../../apis/types/asset/AssetModel";
import { DeviceModel } from "../../../../apis/types/device/DeviceModel";
import { DeviceStatisticsResponseModel } from "../../../../apis/types/device/DeviceStatisticsResponseModel";
import { showDate } from "../../../../services/util/DateUtil";
import AssetSettingsCard from "../AssetSettingsCard";

type Props = {
  asset: AssetModel;
  device: DeviceModel;
};

export default function AssetSettingsDeviceInfo(props: Props) {
  const [deviceStatistics, setDeviceStatistics] = useState<DeviceStatisticsResponseModel>();

  useEffect(() => {
    DeviceApi.statistics(props.device?.id).then((x) => {
      setDeviceStatistics(x);
    });
  }, [props.device]);

  return (
    <>
      {props.device && deviceStatistics && (
        <div className="flex text-sm flex-col">
          <AssetSettingsCard title="Device info">
            <table className="border w-1/2 mb-4">
              <tbody>
                <tr>
                  <td
                    className="p-2 bg-gray-100 text-xs text-gray-700 font-medium border"
                    style={{ width: "120px" }}>
                    IMEI
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
                  <td className="p-2 bg-gray-100 text-xs text-gray-700 font-medium border">
                    Model
                  </td>
                  <td className="p-2 font-medium border">{props.device.deviceType.model}</td>
                </tr>
              </tbody>
            </table>
          </AssetSettingsCard>
          <AssetSettingsCard title="Statistics">
            <table className="border w-1/2">
              <tbody>
                <tr>
                  <td
                    className="p-2 bg-gray-100 text-xs text-gray-700 font-medium border"
                    style={{ width: "150px" }}>
                    Locations
                  </td>
                  <td className="p-2 font-medium border">{deviceStatistics.locations}</td>
                </tr>
                <tr>
                  <td
                    className="p-2 bg-gray-100 text-xs text-gray-700 font-medium border"
                    style={{ width: "150px" }}>
                    Connections
                  </td>
                  <td className="p-2 font-medium border">{deviceStatistics.connections}</td>
                </tr>
                <tr>
                  <td
                    className="p-2 bg-gray-100 text-xs text-gray-700 font-medium border"
                    style={{ width: "150px" }}>
                    First location date
                  </td>
                  <td className="p-2 font-medium border">
                    {showDate(deviceStatistics.firstLocationDateTime)}
                  </td>
                </tr>
                <tr>
                  <td
                    className="p-2 bg-gray-100 text-xs text-gray-700 font-medium border"
                    style={{ width: "150px" }}>
                    Last location date
                  </td>
                  <td className="p-2 font-medium border">
                    {showDate(deviceStatistics.lastLocationDateTime)}
                  </td>
                </tr>
              </tbody>
            </table>
          </AssetSettingsCard>
        </div>
      )}
    </>
  );
}
