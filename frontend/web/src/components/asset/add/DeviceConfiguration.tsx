import { DeviceTypeModel } from "@navtrack/shared/api/model/generated";
import { classNames } from "@navtrack/shared/utils/tailwind";
import { FormattedMessage } from "react-intl";

type DeviceConfigurationProps = {
  deviceType?: DeviceTypeModel;
  className?: string;
};

export function DeviceConfiguration(props: DeviceConfigurationProps) {
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
              {import.meta.env.VITE_LISTENER_HOSTNAME}
            </td>
          </tr>
          <tr>
            <td className="border bg-gray-100 p-2 text-xs font-medium text-gray-700">
              <FormattedMessage id="generic.ip-address" />
            </td>
            <td className="border p-2 font-medium">
              {import.meta.env.VITE_LISTENER_IP}
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
