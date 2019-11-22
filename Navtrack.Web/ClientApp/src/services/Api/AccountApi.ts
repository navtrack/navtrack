import {ApiUtil} from "./ApiUtil";

export const AccountApi = {
    login: function (email: string, password: string): Promise<Response> {
        return fetch(ApiUtil.apiUrl("account/login"), {
            method: "post",
            headers: {
                "Content-Type": "application/json"
            },
            body: JSON.stringify({email, password})
        });
    }
};