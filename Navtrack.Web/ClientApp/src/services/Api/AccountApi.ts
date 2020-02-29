import { HttpClient } from "services/HttpClient/HttpClient";
import { ResponseModel } from "./Model/ResponseModel";
import { UserModel } from "./Model/UserModel";
import { RegisterModel } from "components/Account/Register/RegisterModel";
import { apiUrl } from "services/HttpClient/HttpClientUtil";

export const AccountApi = {
  login: function(email: string, password: string): Promise<ResponseModel> {
    return HttpClient.post("account/login", { email, password });
  },

  logout: function(): Promise<Response> {
    return fetch(apiUrl("account/logout"), {
      method: "post",
      credentials: "include",
      headers: {
        "Content-Type": "application/json"
      }
    });
  },

  get: function(): Promise<UserModel> {
    return HttpClient.get<UserModel>("account");
  },

  register: function(registerModel: RegisterModel): Promise<ResponseModel> {
    return HttpClient.post("account/register", registerModel);
  }
};
