import { HttpClient } from "../services/httpClient/HttpClient";
import { apiUrl } from "../services/httpClient/HttpClientUtil";
import { DeviceTypeModel } from "./types/device/DeviceTypeModel";

export const DeviceTypeApi = {
  getDeviceTypes: function (): Promise<DeviceTypeModel[]> {
    return HttpClient.get<DeviceTypeModel[]>(apiUrl("devices/types"));
  }
};
