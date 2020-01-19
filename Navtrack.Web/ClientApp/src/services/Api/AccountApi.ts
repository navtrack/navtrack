import {HttpClientUtil} from "../HttpClient/HttpClientUtil";
import { HttpClient } from "services/HttpClient/HttpClient";
import { ResponseModel } from "./Model/ResponseModel";

export const AccountApi = {
    login: function (email: string, password: string): Promise<ResponseModel> {
        return HttpClient.post("account/login", {email, password});
    },

    logout: function (): Promise<Response> {
        return fetch(HttpClientUtil.apiUrl("account/logout"), {
            method: "post",
            credentials: "include",
            headers: {
                "Content-Type": "application/json"
            }
        });
    }
};