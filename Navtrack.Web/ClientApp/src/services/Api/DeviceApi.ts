import { HttpClient } from "../HttpClient/HttpClient";
import { DeviceModel } from "./Model/DeviceModel";
import { DeviceTypeModel } from "./Model/DeviceTypeModel";

export const DeviceApi = {
    get: function(id: number) : Promise<DeviceModel> {
        return HttpClient.get<DeviceModel>("devices/" + id);
    },

    getAll: function() : Promise<DeviceModel[]> {
        return HttpClient.get<DeviceModel[]>("devices");
    },

    getTypes: function() : Promise<DeviceTypeModel[]> {
        return HttpClient.get<DeviceTypeModel[]>("devices/types");
    },
    
    getAvailableDevices: function(id?: number) : Promise<DeviceModel[]> {
        const url = id ? `devices/available/${id}` : "devices/available";

        return HttpClient.get<DeviceModel[]>(url);
    },
    
    update: function(device: DeviceModel) : Promise<Response> {
        return HttpClient.put("devices/" + device.id, device);
    },
    
    add: function(device: DeviceModel) : Promise<Response> {
        return HttpClient.post("devices", device);
    }
}