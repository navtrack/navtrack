import { HttpClient } from "../HttpClient/HttpClient";
import { DeviceModel } from "./Model/DeviceModel";
import { DeviceTypeModel } from "./Model/DeviceTypeModel";
import { ResponseModel } from "./Model/ResponseModel";

export const DeviceApi = {
    get: function(id: number) : Promise<DeviceModel> {
        return HttpClient.get<DeviceModel>("devices/" + id);
    },

    getAll: function() : Promise<DeviceModel[]> {
        return HttpClient.get<DeviceModel[]>("devices");
    },

    delete: function(id: number) : Promise<ResponseModel> {
        return HttpClient.delete(`devices/${id}`);
    },

    getTypes: function() : Promise<DeviceTypeModel[]> {
        return HttpClient.get<DeviceTypeModel[]>("devices/types");
    },
    
    getAvailableDevices: function(id?: number) : Promise<DeviceModel[]> {
        const url = id ? `devices/available/${id}` : "devices/available";

        return HttpClient.get<DeviceModel[]>(url);
    },
    
    put: function(device: DeviceModel) : Promise<ResponseModel> {
        return HttpClient.put(`devices/${device.id}`, device);
    },
    
    add: function(device: DeviceModel) : Promise<ResponseModel> {
        return HttpClient.post("devices", device);
    }
};