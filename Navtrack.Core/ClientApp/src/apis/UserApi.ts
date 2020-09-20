import { HttpClient } from "../services/httpClient/HttpClient";
import { apiUrl } from "../services/httpClient/HttpClientUtil";
import { ResponseModel } from "./types/ResponseModel";
import { UserModel } from "./types/user/UserModel";

export const UserApi = {
  get: function (id: number): Promise<UserModel> {
    return HttpClient.get<UserModel>(apiUrl(`users/${id}`));
  },

  getAll: function (): Promise<UserModel[]> {
    return HttpClient.get<UserModel[]>(apiUrl("users"));
  },

  delete: function (id: number): Promise<ResponseModel> {
    return HttpClient.delete(apiUrl(`users/${id}`));
  },

  update: function (asset: UserModel): Promise<ResponseModel> {
    return HttpClient.put(apiUrl(`users/${asset.id}`), asset);
  },

  add: function (asset: UserModel): Promise<ResponseModel> {
    return HttpClient.post(apiUrl("users"), asset);
  }
};
