import React, { useState, useEffect } from "react";
import { AssetModel, DefaultAssetModel } from "services/Api/Model/AssetModel";
import { DeviceModel } from "services/Api/Model/DeviceModel";
import { AppError } from "services/HttpClient/AppError";
import { useHistory } from "react-router";
import { DeviceApi } from "services/Api/DeviceApi";
import { AssetApi } from "services/Api/AssetApi";
import InputError, { HasErrors, AddError, ClearError } from "components/Common/InputError";
import { addNotification } from "components/Notifications";
import AdminLayout from "components/Framework/Layouts/Admin/AdminLayout";
import { ValidationResult } from "components/Common/ValidatonResult";

type Props = {
  id?: number
}

export default function AssetEdit(props: Props) {
  const [asset, setAsset] = useState<AssetModel>(DefaultAssetModel);
  const [devices, setDevices] = useState<DeviceModel[]>([]);
  const [error, setError] = useState<AppError>();
  const [show, setShow] = useState(!props.id);
  const history = useHistory();

  useEffect(() => {
    DeviceApi.getAvailableDevices(props.id)
      .then(devices => setDevices(devices));

    if (props.id) {
      AssetApi.get(props.id)
        .then(x => {
          setAsset(x);
          setShow(true);
        })
        .catch(setError);
    }
  }, [props.id])

  const submitForm = async () => {
    const validationResult = validateModel(asset);

    if (HasErrors(validationResult)) {
      setError(new AppError(validationResult));
    } else {
      if (asset.id > 0) {
        AssetApi.put(asset)
          .then(() => {
            history.push("/assets");
            addNotification("Asset saved successfully.");
          })
          .catch(setError);
      } else {
        AssetApi.add(asset)
          .then(() => {
            history.push("/assets");
            addNotification("Asset added successfully.");
          })
          .catch(setError);
      }
    }
  };

  return (
    <AdminLayout error={error}>
      {show &&
        <div className="shadow rounded bg-white flex flex-col">
          <div className="p-4">
            <div className="font-medium text-lg">{props.id ? <>Edit asset</> : <>Add asset</>}</div>
          </div>
          <div className="p-4">
            <div className="flex flex-row mb-5">
              <div className="w-20 text-gray-700 font-medium h-10 flex items-center">Name</div>
              <div className="text-gray-700 font-medium w-5/12">
                <input className="h-10 shadow bg-gray-100 appearance-none rounded py-2 px-3 text-gray-700 focus:outline-none focus:border focus:border-gray-900 w-full"
                  value={asset.name}
                  onChange={(e) => {
                    setAsset({ ...asset, name: e.target.value });
                    setError(x => ClearError<AssetModel>(x, "name"));
                  }}
                />
                <InputError name="name" errors={error} />
              </div>
            </div>
            <div className="flex flex-row">
              <div className="w-20 text-gray-700 font-medium h-10 flex items-center">Device</div>
              <div className="w-20 text-gray-700 font-medium w-5/12">
                <div className="relative shadow rounded bg-gray-100 w-full">
                  <select className="block appearance-none bg-white px-3 py-2 cursor-pointer focus:outline-none bg-gray-100 w-full"
                    onChange={(e) => setAsset({ ...asset, deviceId: parseInt(e.target.value) })}
                    value={asset.deviceId}>
                    <option value={0} key={0}>None</option>
                    {devices.map(x => <option value={x.id} key={x.id}>{x.type} (IMEI: {x.imei})</option>)}
                  </select>
                  <div className="pointer-events-none absolute inset-y-0 right-0 flex items-center px-2 pt-1">
                    <i className="fas fa-chevron-down" />
                  </div>
                </div>
                <InputError name="deviceId" errors={error} />
              </div>
              <div className="ml-4 text-gray-700 text-sm h-10 flex items-center">Showing unassigned devices.</div>
            </div>
          </div>
          <div className="p-4">
            <button className="shadow-md bg-gray-200 hover:bg-gray-300 py-2 px-4 rounded focus:outline-none"
              onClick={() => history.goBack()}>Cancel</button>
            <button className="shadow-md bg-gray-800 hover:bg-gray-900 text-white py-2 px-4 rounded focus:outline-none ml-3"
              onClick={submitForm}>Save</button>
          </div>
        </div>
      }
    </AdminLayout>
  );
}

const validateModel = (asset: AssetModel): ValidationResult => {
  const validationResult: ValidationResult = {};

  if (asset.name.length === 0) {
    AddError<AssetModel>(validationResult, "name", "The name is required.");
  }

  return validationResult;
};