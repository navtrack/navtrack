import { Api } from "./Api";
import { Protocol } from "./Types/Protocol";

export const ProtocolApi = {
    getAll: function() : Promise<Protocol[]> {
        return Api.get<Protocol[]>("devices/protocols")
    }
}