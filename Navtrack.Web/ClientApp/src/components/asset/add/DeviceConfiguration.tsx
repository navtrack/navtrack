import React from "react";
import classNames from "classnames";
import { DeviceTypeModel } from "../../../api/model";
import { FormattedMessage } from "react-intl";

type Props = {
  deviceType?: DeviceTypeModel;
  className?: string;
};

export default function DeviceConfiguration(props: Props) {
  return (
    <div className={classNames("text-sm", props.className)}>
      <h1 className="font-semibold mb-2">
        <FormattedMessage id="assets.settings.device.configuration.title" />
      </h1>
      <div className="mb-2">
        <FormattedMessage id="assets.settings.device.configuration.description.1" />
        <br />
        <FormattedMessage id="assets.settings.device.configuration.description.2" />
      </div>
      <table className="w-full text-sm rounded border">
        <tbody>
          <tr>
            <td
              className="p-2 bg-gray-100 text-xs text-gray-700 font-medium border"
              style={{ width: "120px" }}>
              <FormattedMessage id="generic.hostname" />
            </td>
            <td className="p-2 font-medium border">
              <FormattedMessage id="navtrack.listener.hostname" />
            </td>
          </tr>
          <tr>
            <td className="p-2 bg-gray-100 text-xs text-gray-700 font-medium border">
              <FormattedMessage id="generic.ip-address" />
            </td>
            <td className="p-2 font-medium border">
              <FormattedMessage id="navtrack.listener.ip-address" />
            </td>
          </tr>
          <tr>
            <td className="p-2 bg-gray-100 text-xs text-gray-700 font-medium border">
              <FormattedMessage id="generic.port" />
            </td>
            <td className="p-2 font-medium border">
              {props.deviceType ? (
                props.deviceType.protocol.port
              ) : (
                <FormattedMessage id="assets.settings.device.configuration.choose-device" />
              )}
            </td>
          </tr>
        </tbody>
      </table>
    </div>
  );
}
