import { Api } from "./Api";
import { Device } from "./Types/Device";
import { DeviceType } from "./Types/DeviceType";

export const DeviceApi = {
    get: function(id: number) : Promise<Device> {
        return Api.get<Device>("devices/" + id);
    },

    getAll: function() : Promise<Device[]> {
        return Api.get<Device[]>("devices");
    },

    getTypes: function() : Promise<DeviceType[]> {
        return Api.get<DeviceType[]>("devices/types");
    },
    
    getAvailableDevices: function(id?: number) : Promise<Device[]> {
        const url = id ? `devices/available/${id}` : "devices/available";

        return Api.get<Device[]>(url);
    },
    
    update: function(device: Device) : Promise<Response> {
        return Api.put("devices/" + device.id, device);
    },
    
    add: function(device: Device) : Promise<Response> {
        return Api.post("devices", device);
    }
}