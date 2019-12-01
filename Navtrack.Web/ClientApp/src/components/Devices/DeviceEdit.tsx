import React, { useState, useEffect } from "react";
import { DeviceModel, DefaultDeviceModel } from "../../services/Api/Model/DeviceModel";
import { DeviceTypeModel } from "../../services/Api/Model/DeviceTypeModel";
import { DeviceApi } from "../../services/Api/DeviceApi";
import { useHistory } from "react-router";
import AdminLayout from "../Layouts/Admin/AdminLayout";
import InputError, { AddError, HasErrors } from "../Common/InputError";
import { ApiError, Errors } from "../../services/HttpClient/HttpClient";
import { addNotification } from "../Notifications";

type Props = {
    id?: number
}

export default function DeviceEdit(props: Props) {
    const [device, setDevice] = useState<DeviceModel>(DefaultDeviceModel);
    const [deviceTypes, setDeviceTypes] = useState<DeviceTypeModel[]>([]);
    const [errors, setErrors] = useState<Errors>({});
    const [show, setShow] = useState(!props.id);
    const history = useHistory();

    useEffect(() => {
        DeviceApi.getTypes().then(deviceTypes => setDeviceTypes(deviceTypes));

        if (props.id) {
            DeviceApi.get(props.id).then(x => {
                setDevice(x);
                setShow(true);
            });
        }
    }, [props.id]);

    const submitForm = async () => {
        const errors = validateModel(device);

        if (HasErrors(errors)) {
            setErrors(errors);
        } else {
            if (device.id > 0) {
                DeviceApi.update(device)
                    .then(() => {
                        history.push("/devices");
                        addNotification("Device saved successfully.");
                    })
                    .catch((error: ApiError) => {
                        setErrors(error.errors)
                    });
            } else {
                DeviceApi.add(device)
                    .then(() => {
                        history.push("/devices");
                        addNotification("Device added successfully.");
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
                                <h3 className="mb-0">{props.id ? <>Edit device</> : <>Add device</>}</h3>
                            </div>
                        </div>
                    </div>
                    <div className="card-body bg-secondary">
                        <div className="form-group row">
                            <label className="col-md-1 col-form-label form-control-label">IMEI</label>
                            <div className="col-md-6">
                                <input className="form-control form-control-alternative" type="text" value={device.imei}
                                    placeholder="IMEI"
                                    onChange={(e) => {
                                        setDevice({ ...device, imei: e.target.value });
                                        setErrors({ ...errors, imei: [] });
                                    }} />
                                <InputError name="imei" errors={errors} />
                            </div>
                        </div>
                        <div className="form-group row">
                            <label className="col-md-1 col-form-label form-control-label">Name</label>
                            <div className="col-md-6">
                                <input className="form-control form-control-alternative" type="text" value={device.name}
                                    placeholder="Name"
                                    onChange={(e) => {
                                        setDevice({ ...device, name: e.target.value });
                                        setErrors({ ...errors, name: [] });
                                    }} />
                                <InputError name="name" errors={errors} />
                            </div>
                        </div>
                        <div className="form-group row">
                            <label className="col-md-1 col-form-label form-control-label">Type</label>
                            <div className="col-md-6">
                                <select className="form-control form-control-alternative" value={device.deviceTypeId}
                                    onChange={(e) => setDevice({ ...device, deviceTypeId: parseInt(e.target.value) })}>
                                    <option value={0} key={0}>Select a device type</option>
                                    {deviceTypes.map(x => <option value={x.id} key={x.id}>{x.name}</option>)}
                                </select>
                                <InputError name="deviceTypeId" errors={errors} />
                            </div>
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
                </div>}
        </AdminLayout>
    );
}

const validateModel = (device: DeviceModel): Record<string, string[]> => {
    const errors: Record<string, string[]> = {};

    if (device.name.length === 0) {
        AddError(errors, "name", "The Name field is required.");
    }
    if (device.imei.length === 0) {
        AddError(errors, "imei", "The IMEI field is required.");
    }
    else if (device.imei.length < 15) {
        AddError(errors, "imei", "The IMEI field minimum length is 15 characters.");
    }
    if (device.deviceTypeId <= 0) {
        AddError(errors, "deviceTypeId", "The device type field is required.");
    }

    return errors;
};