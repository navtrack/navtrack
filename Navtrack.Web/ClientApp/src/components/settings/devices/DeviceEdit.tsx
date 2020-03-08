import React, { useState, useEffect } from "react";
import { DeviceModel, DefaultDeviceModel } from "services/api/types/device/DeviceModel";
import { DeviceTypeModel } from "services/api/types/device/DeviceTypeModel";
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

type Props = {
  id?: number;
};

export default function DeviceEdit(props: Props) {
  const [device, setDevice] = useState<DeviceModel>(DefaultDeviceModel);
  const [deviceTypes, setDeviceTypes] = useState<DeviceTypeModel[]>([]);
  const [error, setError] = useState<AppError>();
  const [show, setShow] = useState(!props.id);
  const history = useHistory();

  useEffect(() => {
    DeviceApi.getTypes().then(deviceTypes => setDeviceTypes(deviceTypes));

    if (props.id) {
      DeviceApi.get(props.id)
        .then(x => {
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
              name="IMEI"
              value={device.imei}
              errorKey="imei"
              error={error}
              className="mb-3"
              onChange={e => {
                setDevice({ ...device, imei: e.target.value });
                setError(x => ClearError<DeviceModel>(x, "imei"));
              }}
            />
            <TextInput
              name="Name"
              value={device.name}
              errorKey="name"
              error={error}
              className="mb-3"
              onChange={e => {
                setDevice({ ...device, name: e.target.value });
                setError(x => ClearError<DeviceModel>(x, "name"));
              }}
            />
            <DropdownInput
              name="Type"
              value={device.deviceTypeId}
              errorKey="deviceTypeId"
              error={error}
              onChange={e => setDevice({ ...device, deviceTypeId: parseInt(e.target.value) })}>
              <option value={0} key={0}>
                Select a device type
              </option>
              {deviceTypes.map(x => (
                <option value={x.id} key={x.id}>
                  {x.name}
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
  if (device.imei.length === 0) {
    AddError<DeviceModel>(validationResult, "imei", "The IMEI is required.");
  } else if (device.imei.length < 15) {
    AddError<DeviceModel>(validationResult, "imei", "The IMEI must be 15 characters.");
  }
  if (device.deviceTypeId <= 0) {
    AddError<DeviceModel>(validationResult, "deviceTypeId", "The device type is required.");
  }

  return validationResult;
};
