import React, { useState, useEffect } from "react";
import { DeviceModel, DefaultDeviceModel } from "services/Api/Model/DeviceModel";
import { DeviceTypeModel } from "services/Api/Model/DeviceTypeModel";
import { AppError } from "services/HttpClient/AppError";
import { useHistory } from "react-router";
import { DeviceApi } from "services/Api/DeviceApi";
import InputError, { HasErrors, AddError, ClearError } from "components/Common/InputError";
import { addNotification } from "components/Notifications";
import AdminLayout from "components/Framework/Layouts/Admin/AdminLayout";
import { ValidationResult } from "components/Common/ValidatonResult";

type Props = {
  id?: number
}

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
      {show &&
        <div className="shadow rounded bg-white flex flex-col">
          <div className="p-5">
            <div className="font-medium text-lg">{props.id ? <>Edit device</> : <>Add device</>}</div>
          </div>
          <div className="p-5">
            <div className="flex flex-row mb-5">
              <div className="w-20 text-gray-700 font-medium h-10 flex items-center">IMEI</div>
              <div className="text-gray-700 font-medium w-5/12">
                <input className="h-10 shadow bg-gray-100 appearance-none rounded py-2 px-3 text-gray-700 focus:outline-none focus:border focus:border-gray-900 w-full"
                  value={device.imei}
                  placeholder="IMEI"
                  onChange={(e) => {
                    setDevice({ ...device, imei: e.target.value });
                    setError(x => ClearError<DeviceModel>(x, "imei"));
                  }} />
                <InputError name="imei" errors={error} />
              </div>
            </div>
            <div className="flex flex-row mb-5">
              <div className="w-20 text-gray-700 font-medium h-10 flex items-center">Name</div>
              <div className="text-gray-700 font-medium w-5/12">
                <input className="h-10 shadow bg-gray-100 appearance-none rounded py-2 px-3 text-gray-700 focus:outline-none focus:border focus:border-gray-900 w-full"
                  value={device.name}
                  placeholder="Name"
                  onChange={(e) => {
                    setDevice({ ...device, name: e.target.value });
                    setError(x => ClearError<DeviceModel>(x, "name"));
                  }} />
                <InputError name="name" errors={error} />
              </div>
            </div>
            <div className="flex flex-row">
              <div className="w-20 text-gray-700 font-medium h-10 flex items-center">Type</div>
              <div className="w-20 text-gray-700 font-medium w-5/12">
                <div className="relative shadow rounded bg-gray-100 w-full">
                  <select className="block appearance-none bg-white px-3 py-2 cursor-pointer focus:outline-none bg-gray-100 w-full"
                    value={device.deviceTypeId}
                    onChange={(e) => setDevice({ ...device, deviceTypeId: parseInt(e.target.value) })}>
                    <option value={0} key={0}>Select a device type</option>
                    {deviceTypes.map(x => <option value={x.id} key={x.id}>{x.name}</option>)}
                  </select>
                  <div className="pointer-events-none absolute inset-y-0 right-0 flex items-center px-2 pt-1">
                    <i className="fas fa-chevron-down" />
                  </div>
                </div>
                <InputError name="deviceTypeId" errors={error} />
              </div>
              <div className="ml-4 text-gray-700 text-sm h-10 flex items-center">Showing unassigned devices.</div>
            </div>
          </div>
          <div className="p-5">
            <button className="shadow-md bg-gray-200 hover:bg-gray-300 py-2 px-4 rounded focus:outline-none"
              onClick={() => history.goBack()}>Cancel</button>
            <button className="shadow-md bg-gray-800 hover:bg-gray-900 text-white py-2 px-4 rounded focus:outline-none ml-3"
              onClick={submitForm}>Save</button>
          </div>
        </div>}
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
  }
  else if (device.imei.length < 15) {
    AddError<DeviceModel>(validationResult, "imei", "The IMEI must be 15 characters.");
  }
  if (device.deviceTypeId <= 0) {
    AddError<DeviceModel>(validationResult, "deviceTypeId", "The device type is required.");
  }

  return validationResult;
};