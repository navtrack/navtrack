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
import { ChangeDeviceRequestModel } from "apis/types/asset/ChangeDeviceRequestModel";
import { DeviceTypeModel } from "apis/types/device/DeviceTypeModel";
import { AssetApi } from "apis/AssetApi";
import DeviceConfiguration from "components/devices/DeviceConfiguration";
import { Link } from "react-router-dom";
import AssetSettingsCard from "../AssetSettingsCard";

type Props = {
  asset: AssetModel;
  refreshAsset: () => void;
};

export default function AssetDevice() {
  const [model, setModel] = useState<ChangeDeviceRequestModel>();
  const [validate, validationResult, setErrors] = useNewValidation(validateModel);
  const intl = useIntl();
  const [deviceTypes, setDeviceTypes] = useState<DeviceTypeModel[]>();
  const [selectedDeviceType, setSelectedDeviceType] = useState<DeviceTypeModel>();

  const handleSubmit = async () => {
    // if (validate(model)) {
    //   AssetApi.changeDevice(props.asset.id, model)
    //     .then(() => {
    //       // history.push(`/assets/${response.id}/live`);
    //       addNotification(intl.formatMessage({ id: "assets.add.success.notification" }));
    //       props.refreshAsset();
    //     })
    //     .catch(setErrors);
    // }
  };

  return (
    <>
      {deviceTypes && (
        <>
          <AssetSettingsCard title="History" className="mt-4">
            <div className="flex flex-col">
              <span className="text-sm mt-1 mb-3">
                An asset can have only one active device, below is the list of devices that have
                been assigned to this asset and have location data.
              </span>
              <table className="w-full text-sm rounded border">
                <tbody>
                  <tr className="bg-gray-100 text-xs uppercase text-gray-700 p-2 border rounded-t-md font-medium">
                    <td className="p-2 rounded-t">Device ID</td>
                    <td className="p-2">Device Type</td>
                    <td className="p-2">Locations</td>
                    <td></td>
                  </tr>
                </tbody>
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
