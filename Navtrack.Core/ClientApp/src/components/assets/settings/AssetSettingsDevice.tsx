import React, { useState, useEffect } from "react";
import SelectInput from "components/library/forms/SelectInput";
import { AssetModel } from "apis/types/asset/AssetModel";
import { useNewValidation } from "framework/validation/useValidationHook";
import { useIntl } from "react-intl";
import { DeviceTypeApi } from "apis/DeviceTypeApi";
import { addNotification } from "components/library/notifications/Notifications";
import TextInput from "components/library/forms/TextInput";
import Button from "components/library/elements/Button";
import { Validator } from "framework/validation/Validator";
import AssetSettingsCard from "./AssetSettingsCard";
import { ChangeDeviceRequestModel } from "apis/types/asset/ChangeDeviceRequestModel";
import { DeviceTypeModel } from "apis/types/device/DeviceTypeModel";
import { AssetApi } from "apis/AssetApi";

type Props = {
  asset: AssetModel;
  refreshAsset: () => void;
};

export default function AssetSettingsDevice(props: Props) {
  const [model, setModel] = useState<ChangeDeviceRequestModel>({
    deviceId: props.asset.activeDevice.deviceId,
    deviceTypeId: props.asset.activeDevice.deviceType.id
  });
  const [validate, validationResult, setErrors] = useNewValidation(validateModel);
  const intl = useIntl();
  const [deviceTypes, setDeviceTypes] = useState<DeviceTypeModel[]>();

  useEffect(() => {
    DeviceTypeApi.getDeviceTypes().then((deviceTypes) => {
      setDeviceTypes(deviceTypes);
    });
  }, []);

  const handleSubmit = async () => {
    if (validate(model)) {
      AssetApi.changeDevice(props.asset.id, model)
        .then(() => {
          // history.push(`/assets/${response.id}/live`);
          addNotification(intl.formatMessage({ id: "assets.add.success.notification" }));
          props.refreshAsset();
        })
        .catch(setErrors);
    }
  };

  return (
    <>
      {deviceTypes && (
        <>
          <AssetSettingsCard title="Device">
            <div className="w-1/2 flex">
              <div className="flex-grow">
                <SelectInput
                  name={intl.formatMessage({ id: "assets.add.deviceTypeId" })}
                  value={model.deviceTypeId}
                  validationResult={validationResult.property.deviceTypeId}
                  onChange={(e) => setModel({ ...model, deviceTypeId: parseInt(e.target.value) })}>
                  <option value={0} key={0}>
                    {intl.formatMessage({ id: "assets.add.deviceTypeId.placeholder" })}
                  </option>
                  {deviceTypes.map((x) => (
                    <option value={x.id} key={x.id}>
                      {x.displayName}
                    </option>
                  ))}
                </SelectInput>
                <TextInput
                  name={intl.formatMessage({ id: "assets.add.deviceId" })}
                  value={model.deviceId}
                  validationResult={validationResult.property.deviceId}
                  placeholder={intl.formatMessage({ id: "assets.add.deviceId.placeholder" })}
                  className="mb-3"
                  onChange={(e) => {
                    setModel({ ...model, deviceId: e.target.value });
                  }}
                />
                <Button
                  color="primary"
                  onClick={handleSubmit}
                  disabled={validationResult.HasErrors()}>
                  Change device
                </Button>
              </div>
            </div>
          </AssetSettingsCard>
          <AssetSettingsCard title="History" className="mt-4">
            <div className="flex flex-col">
              <span className="text-sm mt-1 mb-3">
                An asset can have only one active device, below is the list of devices that have
                been assigned to this asset and have location data.
              </span>
              <table className="w-full text-sm rounded border">
                <tr className="bg-gray-100 text-xs uppercase text-gray-700 p-2 border rounded-t-md font-medium">
                  <td className="p-2 rounded-t">Device ID</td>
                  <td className="p-2">Device Type</td>
                  <td className="p-2">Locations</td>
                  <td></td>
                </tr>
                {props.asset.devices.map((x) => (
                  <tr className="border">
                    <td className="p-2">{x.deviceId}</td>
                    <td className="p-2">{x.deviceType.displayName}</td>
                    <td className="p-2">{x.locationsCount}</td>
                    <td className="p-2 text-right">
                      {props.asset.activeDevice.id === x.id && (
                        <div className="text-xs bg-gray-900 text-white inline-block px-4 py-1 rounded-lg font-semibold text-white tracking-wide">
                          Active
                        </div>
                      )}
                    </td>
                  </tr>
                ))}
              </table>
            </div>
          </AssetSettingsCard>
        </>
      )}
    </>
  );
}

const validateModel: Validator<ChangeDeviceRequestModel> = (object, validationResult, intl) => {
  if (!object.deviceTypeId || !(object.deviceTypeId > 0)) {
    validationResult.AddError(
      "deviceTypeId",
      intl.formatMessage({ id: "assets.add.deviceTypeId.required" })
    );
  }

  if (!object.deviceId) {
    validationResult.AddError(
      "deviceId",
      intl.formatMessage({ id: "assets.add.deviceId.required" })
    );
  }
};
