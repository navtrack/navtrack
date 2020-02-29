import { HttpClient } from "../HttpClient/HttpClient";
import { AssetModel } from "./Model/AssetModel";
import { ResponseModel } from "./Model/ResponseModel";
import { apiUrl } from "services/HttpClient/HttpClientUtil";

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
