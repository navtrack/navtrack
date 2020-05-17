import { HttpClient } from "../httpClient/HttpClient";
import { apiUrl } from "services/httpClient/HttpClientUtil";
import { DeviceModelModel } from "./types/device/DeviceModelModel";

export const DeviceModelApi = {
  getModels: function (): Promise<DeviceModelModel[]> {
    return HttpClient.get<DeviceModelModel[]>(apiUrl("devicesModels"));
  }
};
