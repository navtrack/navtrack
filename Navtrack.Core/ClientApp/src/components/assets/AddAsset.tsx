import React, { useState, useEffect } from "react";
import { useHistory } from "react-router";
import { AssetApi } from "apis/AssetApi";
import TextInput from "components/library/forms/TextInput";
import { DeviceTypeApi } from "apis/DeviceTypeApi";
import { useIntl, FormattedMessage } from "react-intl";
import Form from "components/library/forms/Form";
import SelectInput from "components/library/forms/SelectInput";
import { useNewValidation } from "framework/validation/useValidationHook";
import { Validator } from "framework/validation/Validator";
import { addNotification } from "components/library/notifications/Notifications";
import Button from "components/library/elements/Button";
import { DeviceTypeModel } from "apis/types/device/DeviceTypeModel";
import {
  DefaultAddAssetRequestModel,
  AddAssetRequestModel
} from "apis/types/asset/AddAssetRequestModel";
import DeviceConfiguration from "components/devices/DeviceConfiguration";

export default function AddAsset() {
  const [asset, setAsset] = useState(DefaultAddAssetRequestModel);
  const [validate, validationResult, setErrors] = useNewValidation(validateAsset);
  const history = useHistory();
  const intl = useIntl();
  const [deviceTypes, setDeviceTypes] = useState<DeviceTypeModel[]>([]);
  const [selectedDeviceType, setSelectedDeviceType] = useState<DeviceTypeModel>();

  useEffect(() => {
    DeviceTypeApi.getDeviceTypes().then((deviceTypes) => {
      setDeviceTypes(deviceTypes);
    });
  }, []);

  useEffect(() => {
    if (deviceTypes) {
      const deviceType = deviceTypes.find((x) => x.id === asset.deviceTypeId);

      setSelectedDeviceType(deviceType);
      console.log(deviceType);
    }
  }, [asset.deviceTypeId, deviceTypes]);

  const handleSubmit = async () => {
    if (validate(asset)) {
      AssetApi.add(asset)
        .then((response) => {
          history.push(`/assets/${response.id}/live`);
          addNotification(intl.formatMessage({ id: "assets.add.success.notification" }));
        })
        .catch(setErrors);
    }
  };

  return (
    <Form
      title={intl.formatMessage({ id: "assets.add.title" })}
      actions={
        <Button color="primary" onClick={handleSubmit} disabled={validationResult.HasErrors()}>
          <FormattedMessage id="assets.add.action" />
        </Button>
      }
      rightChildren={<DeviceConfiguration deviceType={selectedDeviceType} />}>
      <TextInput
        name={intl.formatMessage({ id: "assets.add.name" })}
        value={asset.name}
        validationResult={validationResult.property.name}
        className="mb-3"
        onChange={(e) => {
          setAsset({ ...asset, name: e.target.value });
        }}
      />
      <SelectInput
        name={intl.formatMessage({ id: "assets.add.deviceTypeId" })}
        value={asset.deviceTypeId}
        validationResult={validationResult.property.deviceTypeId}
        onChange={(e) => {
          setAsset({ ...asset, deviceTypeId: parseInt(e.target.value) });
          setSelectedDeviceType(deviceTypes.find((x) => x.id === parseInt(e.target.value)));
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
        name={intl.formatMessage({ id: "assets.add.deviceId" })}
        value={asset.deviceId}
        validationResult={validationResult.property.deviceId}
        placeholder={intl.formatMessage({ id: "assets.add.deviceId.placeholder" })}
        className="mb-3"
        onChange={(e) => {
          setAsset({ ...asset, deviceId: e.target.value });
        }}
      />
    </Form>
  );
}

const validateAsset: Validator<AddAssetRequestModel> = (object, validationResult, intl) => {
  if (!object.name || object.name.length === 0) {
    validationResult.AddError("name", intl.formatMessage({ id: "assets.add.name.required" }));
  }

  if (!(object.deviceTypeId > 0)) {
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
