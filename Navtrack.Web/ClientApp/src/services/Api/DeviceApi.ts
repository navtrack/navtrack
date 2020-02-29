import { HttpClient } from "../HttpClient/HttpClient";
import { DeviceModel } from "./Model/DeviceModel";
import { DeviceTypeModel } from "./Model/DeviceTypeModel";
import { ResponseModel } from "./Model/ResponseModel";
import { apiUrl } from "services/HttpClient/HttpClientUtil";

export const DeviceApi = {
  get: function(id: number): Promise<DeviceModel> {
    return HttpClient.get<DeviceModel>(apiUrl(`devices/${id}`));
  },

  getAll: function(): Promise<DeviceModel[]> {
    return HttpClient.get<DeviceModel[]>(apiUrl("devices"));
  },

  delete: function(id: number): Promise<ResponseModel> {
    return HttpClient.delete(apiUrl(`devices/${id}`));
  },

  getTypes: function(): Promise<DeviceTypeModel[]> {
    return HttpClient.get<DeviceTypeModel[]>(apiUrl("devices/types"));
  },

  getAvailableDevices: function(id?: number): Promise<DeviceModel[]> {
    const url = id ? `devices/available/${id}` : "devices/available";

    return HttpClient.get<DeviceModel[]>(apiUrl(url));
  },

  put: function(device: DeviceModel): Promise<ResponseModel> {
    return HttpClient.put(apiUrl(`devices/${device.id}`), device);
  },

  add: function(device: DeviceModel): Promise<ResponseModel> {
    return HttpClient.post(apiUrl("devices"), device);
  }
};
