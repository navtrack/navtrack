import { HttpClient } from "../httpClient/HttpClient";
import { apiUrl } from "services/httpClient/HttpClientUtil";
import { DeviceModel } from "./types/device/DeviceModel";
import { ResponseModel } from "./types/ResponseModel";

export const DeviceApi = {
  get: function (id: number): Promise<DeviceModel> {
    return HttpClient.get<DeviceModel>(apiUrl(`devices/${id}`));
  },

  getAll: function (): Promise<DeviceModel[]> {
    return HttpClient.get<DeviceModel[]>(apiUrl("devices"));
  },

  delete: function (id: number): Promise<ResponseModel> {
    return HttpClient.delete(apiUrl(`devices/${id}`));
  },

  getAvailableDevices: function (id?: number): Promise<DeviceModel[]> {
    const url = id ? `devices/available/${id}` : "devices/available";

    return HttpClient.get<DeviceModel[]>(apiUrl(url));
  },

  put: function (device: DeviceModel): Promise<ResponseModel> {
    return HttpClient.put(apiUrl(`devices/${device.id}`), device);
  },

  add: function (device: DeviceModel): Promise<ResponseModel> {
    return HttpClient.post(apiUrl("devices"), device);
  }
};
