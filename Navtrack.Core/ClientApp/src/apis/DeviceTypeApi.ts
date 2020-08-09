import { HttpClient } from "framework/httpClient/HttpClient";
import { apiUrl } from "framework/httpClient/HttpClientUtil";
import { DeviceTypeModel } from "./types/device/DeviceTypeModel";

export const DeviceTypeApi = {
  getDeviceTypes: function (): Promise<DeviceTypeModel[]> {
    return HttpClient.get<DeviceTypeModel[]>(apiUrl("devices/types"));
  }
};
