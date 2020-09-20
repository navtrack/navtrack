import { HttpClient } from "../services/httpClient/HttpClient";
import { apiUrl, formatUrl } from "../services/httpClient/HttpClientUtil";
import { AddAssetRequestModel } from "./types/asset/AddAssetRequestModel";
import { AddAssetResponseModel } from "./types/asset/AddAssetResponseModel";
import { AssetModel } from "./types/asset/AssetModel";
import { ChangeDeviceRequestModel } from "./types/asset/ChangeDeviceRequestModel";
import { GetTripsModel } from "./types/asset/GetTripsModel";
import { RenameAssetRequestModel } from "./types/asset/RenameAssetRequestModel";
import { LocationHistoryRequestModel } from "./types/location/LocationHistoryRequestModel";
import { LocationModel } from "./types/location/LocationModel";
import { ResponseModel } from "./types/ResponseModel";

export const AssetApi = {
  get: function (id: number): Promise<AssetModel> {
    return HttpClient.get<AssetModel>(apiUrl(`assets/${id}`));
  },

  getAll: function (): Promise<AssetModel[]> {
    return HttpClient.get<AssetModel[]>(apiUrl("assets"));
  },

  add: function (asset: AddAssetRequestModel): Promise<AddAssetResponseModel> {
    return HttpClient.post(apiUrl("assets"), asset);
  },

  delete: function (id: number): Promise<ResponseModel> {
    return HttpClient.delete(apiUrl(`assets/${id}`));
  },

  rename: function (assetId: number, model: RenameAssetRequestModel) {
    return HttpClient.post(apiUrl(`assets/${assetId}/settings/rename`), model);
  },

  changeDevice: function (
    assetId: number,
    model: ChangeDeviceRequestModel
  ): Promise<ResponseModel> {
    return HttpClient.put(apiUrl(`assets/${assetId}/device`), model);
  },

  getTrips: function (id: number, filter: LocationHistoryRequestModel): Promise<GetTripsModel> {
    return HttpClient.get<GetTripsModel>(formatUrl(apiUrl(`assets/${id}/trips`), filter));
  },

  getLatestLocation: function (assetId: number): Promise<LocationModel> {
    return HttpClient.get<LocationModel>(apiUrl(`assets/${assetId}/locations/latest`));
  },

  getLocations: function (
    assetId: number,
    filter: LocationHistoryRequestModel
  ): Promise<LocationModel[]> {
    return HttpClient.get<LocationModel[]>(
      formatUrl(apiUrl(`assets/${assetId}/locations`), filter)
    );
  }
};
