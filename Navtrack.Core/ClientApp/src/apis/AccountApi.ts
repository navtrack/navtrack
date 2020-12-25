import { RegisterModel } from "../components/register/RegisterModel";
import { HttpClient } from "../services/httpClient/HttpClient";
import { apiUrl } from "../services/httpClient/HttpClientUtil";
import { ResponseModel } from "./types/ResponseModel";

export const AccountApi = {
  register: function (model: RegisterModel): Promise<ResponseModel> {
    return HttpClient.post(apiUrl("account/register"), model);
  }
};
