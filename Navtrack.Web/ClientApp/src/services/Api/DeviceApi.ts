import { Api } from "./Api";
import { Device } from "./Types/Device";

export const DeviceApi = {
    get: function(id: number) : Promise<Device> {
        return Api.get<Device>("devices/" + id);
    },

    getAll: function() : Promise<Device[]> {
        return Api.get<Device[]>("devices")
    },
    
    save: function(device: Device) : Promise<Device> {
        return Api.post<Device>("devices", device);
    }
}