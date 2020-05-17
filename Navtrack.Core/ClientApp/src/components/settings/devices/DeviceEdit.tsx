import React, { useState, useEffect } from "react";
import { DeviceModel, DefaultDeviceModel } from "services/api/types/device/DeviceModel";
import { DeviceModelModel } from "services/api/types/device/DeviceModelModel";
import { AppError } from "services/httpClient/AppError";
import { useHistory } from "react-router";
import { DeviceApi } from "services/api/DeviceApi";
import { HasErrors, ClearError, AddError } from "components/common/InputError";
import { addNotification } from "components/framework/notifications/Notifications";
import AdminLayout from "components/framework/layouts/admin/AdminLayout";
import TextInput from "components/framework/elements/TextInput";
import DropdownInput from "components/framework/elements/DropdownInput";
import Button from "components/framework/elements/Button";
import { ValidationResult } from "components/common/ValidatonResult";
import { DeviceModelApi } from "services/api/DeviceModelApi";

type Props = {
  id?: number;
};

export default function DeviceEdit(props: Props) {
  const [device, setDevice] = useState<DeviceModel>(DefaultDeviceModel);
  const [deviceTypes, setDeviceModels] = useState<DeviceModelModel[]>([]);
  const [error, setError] = useState<AppError>();
  const [show, setShow] = useState(!props.id);
  const history = useHistory();

  useEffect(() => {
    DeviceModelApi.getModels().then((deviceModels) => setDeviceModels(deviceModels));

    if (props.id) {
      DeviceApi.get(props.id)
        .then((x) => {
          setDevice(x);
          setShow(true);
        })
        .catch(setError);
    }
  }, [props.id]);

  const submitForm = async () => {
    const validationResult = validateModel(device);

    if (HasErrors(validationResult)) {
      setError(new AppError(validationResult));
    } else {
      if (device.id > 0) {
        DeviceApi.put(device)
          .then(() => {
            history.push("/devices");
            addNotification("Device saved successfully.");
          })
          .catch(setError);
      } else {
        DeviceApi.add(device)
          .then(() => {
            history.push("/devices");
            addNotification("Device added successfully.");
          })
          .catch(setError);
      }
    }
  };

  return (
    <AdminLayout>
      {show && (
        <div className="shadow rounded bg-white flex flex-col">
          <div className="p-3">
            <div className="font-medium text-lg">
              {props.id ? <>Edit device</> : <>Add device</>}
            </div>
          </div>
          <div className="p-3">
            <TextInput
              name="Device ID"
              value={device.deviceId}
              errorKey="deviceId"
              placeHolder="IMEI / Serial Number / Device ID"
              error={error}
              className="mb-3"
              onChange={(e) => {
                setDevice({ ...device, deviceId: e.target.value });
                setError((x) => ClearError<DeviceModel>(x, "deviceId"));
              }}
            />
            <TextInput
              name="Name"
              value={device.name}
              errorKey="name"
              error={error}
              className="mb-3"
              onChange={(e) => {
                setDevice({ ...device, name: e.target.value });
                setError((x) => ClearError<DeviceModel>(x, "name"));
              }}
            />
            <DropdownInput
              name="Model"
              value={device.deviceModelId}
              errorKey="deviceModelId"
              error={error}
              onChange={(e) => setDevice({ ...device, deviceModelId: parseInt(e.target.value) })}>
              <option value={0} key={0}>
                Select a device model
              </option>
              {deviceTypes.map((x) => (
                <option value={x.id} key={x.id}>
                  {x.displayName}
                </option>
              ))}
            </DropdownInput>
          </div>
          <div className="p-3">
            <Button color="secondary" onClick={history.goBack} className="mr-3">
              Cancel
            </Button>
            <Button color="primary" onClick={submitForm}>
              Save
            </Button>
          </div>
        </div>
      )}
    </AdminLayout>
  );
}

const validateModel = (device: DeviceModel): ValidationResult => {
  const validationResult: ValidationResult = {};

  if (device.name.length === 0) {
    AddError<DeviceModel>(validationResult, "name", "The name is required.");
  }
  if (device.deviceId.length === 0) {
    AddError<DeviceModel>(validationResult, "deviceId", "The Device ID is required.");
  }
  if (device.deviceModelId <= 0) {
    AddError<DeviceModel>(validationResult, "deviceModelId", "The device model is required.");
  }

  return validationResult;
};
