import { HttpClient } from "services/httpClient/HttpClient";
import { RegisterModel } from "components/account/register/RegisterModel";
import { apiUrl } from "services/httpClient/HttpClientUtil";
import { UserModel } from "./types/user/UserModel";
import { ResponseModel } from "./types/ResponseModel";

export const AccountApi = {
  get: function(): Promise<UserModel> {
    return HttpClient.get<UserModel>(apiUrl("account"));
  },

  register: function(model: RegisterModel): Promise<ResponseModel> {
    return HttpClient.post(apiUrl("account/register"), model);
  }
};
