import React, { useState, useEffect } from "react";
import { useIntl } from "react-intl";
import { Link } from "react-router-dom";
import { AssetApi } from "../../../apis/AssetApi";
import { DeviceTypeApi } from "../../../apis/DeviceTypeApi";
import { AssetModel } from "../../../apis/types/asset/AssetModel";
import { ChangeDeviceRequestModel } from "../../../apis/types/asset/ChangeDeviceRequestModel";
import { DeviceTypeModel } from "../../../apis/types/device/DeviceTypeModel";
import { useNewValidation } from "../../../services/validation/useValidationHook";
import { ValidateAction } from "../../../services/validation/ValidateAction";
import DeviceConfiguration from "../../devices/DeviceConfiguration";
import Button from "../../shared/elements/Button";
import SelectInput from "../../shared/forms/SelectInput";
import TextInput from "../../shared/forms/TextInput";
import { addNotification } from "../../shared/notifications/Notifications";
import AssetSettingsCard from "./AssetSettingsCard";

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
  const [selectedDeviceType, setSelectedDeviceType] = useState<DeviceTypeModel>();

  useEffect(() => {
    DeviceTypeApi.getDeviceTypes().then((deviceTypes) => {
      setDeviceTypes(deviceTypes);
    });
  }, []);

  useEffect(() => {
    if (deviceTypes) {
      setSelectedDeviceType(deviceTypes.find((x) => x.id === model.deviceTypeId));
    }
  }, [deviceTypes, model.deviceTypeId]);

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
          <AssetSettingsCard title="Active device">
            <div className="flex">
              <div className="flex-grow w-1/2 pr-4">
                <SelectInput
                  name={intl.formatMessage({ id: "assets.add.deviceTypeId" })}
                  value={model.deviceTypeId}
                  validationResult={validationResult.property.deviceTypeId}
                  onChange={(e) => {
                    setModel({ ...model, deviceTypeId: parseInt(e.target.value) });
                    setSelectedDeviceType(
                      deviceTypes.find((x) => x.id === parseInt(e.target.value))
                    );
                  }}>
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
                  title={intl.formatMessage({ id: "assets.add.deviceId" })}
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
              <DeviceConfiguration className="w-1/2 pl-4" deviceType={selectedDeviceType} />
            </div>
          </AssetSettingsCard>
          <AssetSettingsCard title="Device history" className="mt-4">
            <div className="flex flex-col">
              <span className="text-sm mt-1 mb-3">
                Below is the list of devices that have been assigned to this asset, an asset can
                have only one active device.
              </span>
              <table className="w-full text-sm rounded border">
                <tbody>
                  <tr className="bg-gray-100 text-xs uppercase text-gray-700 p-2 border font-medium">
                    <td className="p-2">Device Type</td>
                    <td className="p-2">IMEI</td>
                    <td className="p-2">Locations</td>
                    <td></td>
                  </tr>
                  {props.asset.devices.map((x) => (
                    <tr className="border" key={x.deviceId}>
                      <td className="p-2">{x.deviceType.displayName}</td>
                      <td className="p-2">
                        <Link
                          to={`/assets/${x.assetId}/settings/device/${x.id}/info`}
                          className="text-blue-700 font-medium">
                          {x.deviceId}
                        </Link>
                      </td>
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
                </tbody>
              </table>
            </div>
          </AssetSettingsCard>
        </>
      )}
    </>
  );
}

const validateModel: ValidateAction<ChangeDeviceRequestModel> = (
  object,
  validationResult,
  intl
) => {
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
