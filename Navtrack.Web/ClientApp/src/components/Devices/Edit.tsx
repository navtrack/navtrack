import React, { useState, useEffect } from "react";
import { Device } from "../../services/Api/Types/Device";
import { DeviceType } from "../../services/Api/Types/DeviceType";
import { DeviceApi } from "../../services/Api/DeviceApi";
import { useHistory } from "react-router";
import AdminLayout from "../AdminLayout";

type Props = {
    id?: number
}

export default function DeviceEdit(props: Props) {
    const [device, setDevice] = useState<Device>({
        id: 0,
        imei: '',
        name: '',
        type: '',
        deviceTypeId: 0
    });
    const [deviceTypes, setDeviceTypes] = useState<DeviceType[]>([]);
    const history = useHistory();

    useEffect(() => {
        if (props.id) {
            DeviceApi.get(props.id).then(x => setDevice(x));
        }

        DeviceApi.getTypes().then(deviceTypes => {
            setDeviceTypes(deviceTypes)
        });
    }, [props.id])

    const submitForm = async () => {
        if (device.id > 0) {
            await DeviceApi.update(device);
        }
        else {
            await DeviceApi.add(device);
        }

        history.goBack();
    }

    return (
        <AdminLayout>
            {device &&
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
                            <div className="col-md-4">
                                <input className="form-control form-control-alternative" type="text" value={device.imei} onChange={(e) => setDevice({ ...device, imei: e.target.value })} />
                            </div>
                        </div>
                        <div className="form-group row">
                            <label className="col-md-1 col-form-label form-control-label">Name</label>
                            <div className="col-md-4">
                                <input className="form-control form-control-alternative" type="text" value={device.name} onChange={(e) => setDevice({ ...device, name: e.target.value })} />
                            </div>
                        </div>
                        <div className="form-group row">
                            <label className="col-md-1 col-form-label form-control-label">Type</label>
                            <div className="col-md-4">
                                <select className="form-control form-control-alternative" value={device.deviceTypeId} onChange={(e) => setDevice({ ...device, deviceTypeId: parseInt(e.target.value) })}>
                                    {deviceTypes.map(x => <option value={x.id} key={x.id}>{x.name}</option>)}
                                </select>
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
                </div>
            }
        </AdminLayout>
    );
}