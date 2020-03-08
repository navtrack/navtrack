import React, { useState, useEffect } from "react";
import { AssetModel, DefaultAssetModel } from "services/Api/Model/AssetModel";
import { DeviceModel } from "services/Api/Model/DeviceModel";
import { AppError } from "services/HttpClient/AppError";
import { useHistory } from "react-router";
import { DeviceApi } from "services/Api/DeviceApi";
import { AssetApi } from "services/Api/AssetApi";
import { HasErrors, AddError, ClearError } from "components/Common/InputError";
import { addNotification } from "components/Notifications";
import AdminLayout from "components/framework/Layouts/Admin/AdminLayout";
import { ValidationResult } from "components/Common/ValidatonResult";
import Button from "components/framework/Elements/Button";
import TextInput from "components/framework/Elements/TextInput";
import DropdownInput from "components/framework/Elements/DropdownInput";

type Props = {
  id?: number;
};

export default function AssetEdit(props: Props) {
  const [asset, setAsset] = useState<AssetModel>(DefaultAssetModel);
  const [devices, setDevices] = useState<DeviceModel[]>([]);
  const [error, setError] = useState<AppError>();
  const [show, setShow] = useState(!props.id);
  const history = useHistory();

  useEffect(() => {
    DeviceApi.getAvailableDevices(props.id).then(devices => setDevices(devices));

    if (props.id) {
      AssetApi.get(props.id)
        .then(x => {
          setAsset(x);
          setShow(true);
        })
        .catch(setError);
    }
  }, [props.id]);

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
      {show && (
        <div className="shadow rounded bg-white flex flex-col">
          <div className="p-3">
            <div className="font-medium text-lg">{props.id ? <>Edit asset</> : <>Add asset</>}</div>
          </div>
          <div className="p-3">
            <TextInput
              name="Name"
              value={asset.name}
              errorKey="name"
              error={error}
              className="mb-3"
              onChange={e => {
                setAsset({ ...asset, name: e.target.value });
                setError(x => ClearError<AssetModel>(x, "name"));
              }}
            />
            <DropdownInput
              name="Device"
              value={asset.deviceId}
              errorKey="deviceId"
              error={error}
              className="mb-3"
              tip="Showing unassigned devices."
              onChange={e => setAsset({ ...asset, deviceId: parseInt(e.target.value) })}>
              <option value={0} key={0}>
                None
              </option>
              {devices.map(x => (
                <option value={x.id} key={x.id}>
                  {x.type} (IMEI: {x.imei})
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

const validateModel = (asset: AssetModel): ValidationResult => {
  const validationResult: ValidationResult = {};

  if (asset.name.length === 0) {
    AddError<AssetModel>(validationResult, "name", "The name is required.");
  }

  return validationResult;
};
