import {HttpClientUtil} from "../HttpClient/HttpClientUtil";

export const AccountApi = {
    login: function (email: string, password: string): Promise<Response> {
        return fetch(HttpClientUtil.apiUrl("account/login"), {
            method: "post",
            headers: {
                "Content-Type": "application/json"
            },
            body: JSON.stringify({email, password})
        });
    },

    logout: function (): Promise<Response> {
        return fetch(HttpClientUtil.apiUrl("account/logout"), {
            method: "post",
            headers: {
                "Content-Type": "application/json"
            }
        });
    }
};