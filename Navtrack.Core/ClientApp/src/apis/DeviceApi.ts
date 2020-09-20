import queryString from "query-string";
import { HttpClient } from "../services/httpClient/HttpClient";
import { apiUrl } from "../services/httpClient/HttpClientUtil";
import { DeviceConnectionResponseModel } from "./types/device/DeviceConnectionResponseModel";
import { DeviceModel } from "./types/device/DeviceModel";
import { DeviceStatisticsResponseModel } from "./types/device/DeviceStatisticsResponseModel";
import { ResponseModel } from "./types/ResponseModel";
import { ResultsPaginationModel } from "./types/ResultsPaginationModel";

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

  connections: function (
    id: number,
    page: number
  ): Promise<ResultsPaginationModel<DeviceConnectionResponseModel>> {
    let params = queryString.stringify({
      page: page
    });

    return HttpClient.get<ResultsPaginationModel<DeviceConnectionResponseModel>>(
      apiUrl(`devices/${id}/connections?${params}`)
    );
  },

  statistics: function (id: number): Promise<DeviceStatisticsResponseModel> {
    return HttpClient.get<DeviceStatisticsResponseModel>(apiUrl(`devices/${id}/statistics`));
  },

  put: function (device: DeviceModel): Promise<ResponseModel> {
    return HttpClient.put(apiUrl(`devices/${device.id}`), device);
  },

  add: function (device: DeviceModel): Promise<ResponseModel> {
    return HttpClient.post(apiUrl("devices"), device);
  }
};
