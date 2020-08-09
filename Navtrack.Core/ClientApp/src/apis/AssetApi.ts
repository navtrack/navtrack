import { HttpClient } from "framework/httpClient/HttpClient";
import { apiUrl } from "framework/httpClient/HttpClientUtil";
import { AssetModel } from "./types/asset/AssetModel";
import { ResponseModel } from "./types/ResponseModel";
import { AddAssetRequestModel } from "./types/asset/AddAssetRequestModel";
import { AddAssetResponseModel } from "./types/asset/AddAssetResponseModel";
import { RenameAssetRequestModel } from "./types/asset/RenameAssetRequestModel";
import { ChangeDeviceRequestModel } from "./types/asset/ChangeDeviceRequestModel";

export const AssetApi = {
  changeDevice: function (assetId: number, model: ChangeDeviceRequestModel): Promise<ResponseModel> {
    return HttpClient.put(apiUrl(`assets/${assetId}/device`), model);
  },


  get: function (id: number): Promise<AssetModel> {
    return HttpClient.get<AssetModel>(apiUrl(`assets/${id}`));
  },

  getAll: function (): Promise<AssetModel[]> {
    return HttpClient.get<AssetModel[]>(apiUrl("assets"));
  },

  delete: function (id: number): Promise<ResponseModel> {
    return HttpClient.delete(apiUrl(`assets/${id}`));
  },

  put: function (asset: AssetModel): Promise<ResponseModel> {
    return HttpClient.put(apiUrl(`assets/${asset.id}`), asset);
  },

  add: function (asset: AddAssetRequestModel): Promise<AddAssetResponseModel> {
    return HttpClient.post(apiUrl("assets"), asset);
  },

  rename: function(assetId: number, model: RenameAssetRequestModel) {
    return HttpClient.post(apiUrl(`assets/${assetId}/settings/rename`), model);
  },

};
