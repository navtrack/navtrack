import { Api } from "./Api";
import { Device } from "./Types/Device";

export const DeviceApi = {
    get: function(id: number) : Promise<Device> {
        return Api.get<Device>("devices/" + id);
    },

    getAll: function() : Promise<Device[]> {
        return Api.get<Device[]>("devices")
    },
    
    update: function(device: Device) : Promise<Response> {
        return Api.put("devices/" + device.id, device);
    },
    
    add: function(device: Device) : Promise<Response> {
        return Api.post("devices", device);
    }
}