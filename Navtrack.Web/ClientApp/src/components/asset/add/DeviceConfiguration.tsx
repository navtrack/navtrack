import { DeviceTypeModel } from "@navtrack/navtrack-app-shared/dist/api/model/generated";
import classNames from "classnames";
import { FormattedMessage } from "react-intl";

interface IDeviceConfiguration {
  deviceType?: DeviceTypeModel;
  className?: string;
}

export default function DeviceConfiguration(props: IDeviceConfiguration) {
  return (
    <div className={classNames("text-sm", props.className)}>
      <h1 className="font-semibold">
        <FormattedMessage id="assets.settings.device.configuration.title" />
      </h1>
      <table className="mt-2 w-full border">
        <tbody>
          <tr>
            <td
              className="border bg-gray-100 p-2 text-xs font-medium text-gray-700"
              style={{ width: "120px" }}>
              <FormattedMessage id="generic.hostname" />
            </td>
            <td className="border p-2 font-medium">
              <FormattedMessage id="navtrack.listener.hostname" />
            </td>
          </tr>
          <tr>
            <td className="border bg-gray-100 p-2 text-xs font-medium text-gray-700">
              <FormattedMessage id="generic.ip-address" />
            </td>
            <td className="border p-2 font-medium">
              <FormattedMessage id="navtrack.listener.ip-address" />
            </td>
          </tr>
          <tr>
            <td className="border bg-gray-100 p-2 text-xs font-medium text-gray-700">
              <FormattedMessage id="generic.port" />
            </td>
            <td className="border p-2 font-medium">
              {props.deviceType ? (
                props.deviceType.protocol.port
              ) : (
                <FormattedMessage id="assets.settings.device.configuration.choose-device" />
              )}
            </td>
          </tr>
        </tbody>
      </table>
      <div className="mt-2">
        <FormattedMessage id="assets.settings.device.configuration.description.1" />
      </div>
      <div>
        <FormattedMessage id="assets.settings.device.configuration.description.2" />
      </div>
    </div>
  );
}
