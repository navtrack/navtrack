import React from "react";

export default function AddDevice() {

    return (
        <>
            <div className="card shadow">
                <div className="card-header">
                    <div className="row align-items-center">
                        <div className="col">
                            <h3 className="mb-0">Add new device</h3>
                        </div>
                    </div>
                </div>
                <div className="card-body bg-secondary">
                    <div className="form-group row">
                        <label className="col-md-1 col-form-label form-control-label">IMEI</label>
                        <div className="col-md-4">
                            <input className="form-control form-control-alternative" type="text" />
                        </div>
                    </div>
                    <div className="form-group row">
                        <label className="col-md-1 col-form-label form-control-label">Device</label>
                        <div className="col-md-4">
                            <input className="form-control form-control-alternative" type="text" />
                        </div>
                    </div>
                    <div className="form-group row">
                        <label className="col-md-1 col-form-label form-control-label">IMEI</label>
                        <div className="col-md-4">
                            <input className="form-control form-control-alternative" type="text" />
                        </div>
                    </div>
                </div>
                <div className="card-footer">
                    <div className="row align-items-center">
                        <div className="col">
                            <button className="btn btn-primary">Save</button>
                        </div>
                    </div>
                </div>
            </div>
        </>
    );
}