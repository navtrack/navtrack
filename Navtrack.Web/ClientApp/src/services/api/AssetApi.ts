import { HttpClient } from "../httpClient/HttpClient";
import { apiUrl } from "services/httpClient/HttpClientUtil";
import { AssetModel } from "./types/asset/AssetModel";
import { ResponseModel } from "./types/ResponseModel";

export const AssetApi = {
  get: function(id: number): Promise<AssetModel> {
    return HttpClient.get<AssetModel>(apiUrl(`assets/${id}`));
  },

  getAll: function(): Promise<AssetModel[]> {
    return HttpClient.get<AssetModel[]>(apiUrl("assets"));
  },

  delete: function(id: number): Promise<ResponseModel> {
    return HttpClient.delete(apiUrl(`assets/${id}`));
  },

  put: function(asset: AssetModel): Promise<ResponseModel> {
    return HttpClient.put(apiUrl(`assets/${asset.id}`), asset);
  },

  add: function(asset: AssetModel): Promise<ResponseModel> {
    return HttpClient.post(apiUrl("assets"), asset);
  }
};
