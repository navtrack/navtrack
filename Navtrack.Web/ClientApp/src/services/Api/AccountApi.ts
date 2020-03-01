import { HttpClient } from "services/HttpClient/HttpClient";
import { ResponseModel } from "./Model/ResponseModel";
import { UserModel } from "./Model/UserModel";
import { RegisterModel } from "components/Account/Register/RegisterModel";
import { apiUrl } from "services/HttpClient/HttpClientUtil";

export const AccountApi = {
  get: function(): Promise<UserModel> {
    return HttpClient.get<UserModel>(apiUrl("account"));
  },

  register: function(model: RegisterModel): Promise<ResponseModel> {
    return HttpClient.post(apiUrl("account/register"), model);
  }
};
