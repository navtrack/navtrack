import React, { useState, useEffect } from "react";
import { DeviceApi } from "../../services/Api/DeviceApi";
import { useHistory } from "react-router";
import AdminLayout from "../Layouts/Admin/AdminLayout";
import { AssetModel, DefaultAssetModel } from "../../services/Api/Model/AssetModel";
import { AssetApi } from "../../services/Api/AssetApi";
import { DeviceModel } from "../../services/Api/Model/DeviceModel";
import InputError, { AddError, HasErrors } from "../Common/InputError";
import { Errors, ApiError } from "../../services/HttpClient/HttpClient";
import { addNotification } from "../Notifications";

type Props = {
    id?: number
}

export default function AssetEdit(props: Props) {
    const [asset, setAsset] = useState<AssetModel>(DefaultAssetModel);
    const [devices, setDevices] = useState<DeviceModel[]>([]);
    const [errors, setErrors] = useState<Errors>({});
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
                });
        }
    }, [props.id])

    const submitForm = async () => {
        const errors = validateModel(asset);

        if (HasErrors(errors)) {
            setErrors(errors);
        } else {
            if (asset.id > 0) {
                AssetApi.update(asset)
                    .then(() => {
                        history.push("/assets");
                        addNotification("Asset saved successfully.");
                    })
                    .catch((error: ApiError) => {
                        setErrors(error.errors)
                    });
            } else {
                AssetApi.add(asset)
                    .then(() => {
                        history.push("/assets");
                        addNotification("Asset added successfully.");
                    })
                    .catch((error: ApiError) => {
                        setErrors(error.errors);
                    });
            }
        }
    };

    return (
        <AdminLayout>
            {show &&
                <div className="card shadow">
                    <div className="card-header">
                        <div className="row align-items-center">
                            <div className="col">
                                <h3 className="mb-0">{props.id ? <>Edit asset</> : <>Add asset</>}</h3>
                            </div>
                        </div>
                    </div>
                    <div className="card-body bg-secondary">
                        <div className="form-group row">
                            <label className="col-md-1 col-form-label form-control-label">Name</label>
                            <div className="col-md-5">
                                <input className="form-control form-control-alternative" type="text" value={asset.name} onChange={(e) => {
                                    setAsset({ ...asset, name: e.target.value });
                                    setErrors({ ...errors, name: [] })
                                }} />
                                <InputError name="name" errors={errors} />
                            </div>
                        </div>
                        <div className="form-group row">
                            <label className="col-md-1 col-form-label form-control-label">Device</label>
                            <div className="col-md-5">
                                <select className="form-control form-control-alternative" value={asset.deviceId} onChange={(e) => setAsset({ ...asset, deviceId: parseInt(e.target.value) })}>
                                    <option value={0} key={0}>None</option>
                                    {devices.map(x => <option value={x.id} key={x.id}>{x.type} (IMEI: {x.imei})</option>)}
                                </select>
                            </div>
                            <label className="col-md-6 col-form-label pl-0">Showing unassigned devices.</label>
                        </div>
                    </div>
                    <div className="card-footer">
                        <div className="row align-items-center">
                            <div className="col">
                                <button className="btn btn-secondary" onClick={() => history.goBack()}>Cancel</button>
                                <button className="btn btn-default" onClick={submitForm}>Save</button>
                            </div>
                        </div>
                    </div>
                </div>
            }
        </AdminLayout>
    );
}


const validateModel = (asset: AssetModel): Record<string, string[]> => {
    const errors: Record<string, string[]> = {};

    if (asset.name.length === 0) {
        AddError(errors, "name", "The Name field is required.");
    }

    return errors;
};