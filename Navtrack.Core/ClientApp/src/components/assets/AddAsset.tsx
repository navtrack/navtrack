import React, { useState, useEffect } from "react";
import { useIntl, FormattedMessage } from "react-intl";
import { useHistory } from "react-router";
import { AssetApi } from "../../apis/AssetApi";
import { DeviceTypeApi } from "../../apis/DeviceTypeApi";
import {
  DefaultAddAssetRequestModel,
  AddAssetRequestModel
} from "../../apis/types/asset/AddAssetRequestModel";
import { DeviceTypeModel } from "../../apis/types/device/DeviceTypeModel";
import { useNewValidation } from "../../services/validation/useValidationHook";
import { ValidateAction } from "../../services/validation/ValidateAction";
import DeviceConfiguration from "../devices/DeviceConfiguration";
import Button from "../shared/elements/Button";
import Form from "../shared/forms/Form";
import SelectInput from "../shared/forms/SelectInput";
import TextInput from "../shared/forms/TextInput";
import { addNotification } from "../shared/notifications/Notifications";

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
        title={intl.formatMessage({ id: "assets.add.name" })}
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
        title={intl.formatMessage({ id: "assets.add.deviceId" })}
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

const validateAsset: ValidateAction<AddAssetRequestModel> = (object, validationResult, intl) => {
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
