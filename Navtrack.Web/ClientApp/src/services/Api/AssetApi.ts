import { HttpClient } from "../HttpClient/HttpClient";
import { AssetModel } from "./Model/AssetModel";
import { ResponseModel } from "./Model/ResponseModel";

export const AssetApi = {
    get: function(id: number) : Promise<AssetModel> {
        return HttpClient.get<AssetModel>("assets/" + id);
    },

    getAll: function() : Promise<AssetModel[]> {
        return HttpClient.get<AssetModel[]>("assets");
    },

    delete: function(id: number) : Promise<ResponseModel> {
        return HttpClient.delete("assets/" + id);
    },

    update: function(asset: AssetModel) : Promise<ResponseModel> {
        return HttpClient.put("assets/" + asset.id, asset);
    },
    
    add: function(asset: AssetModel) : Promise<ResponseModel> {
        return HttpClient.post("assets", asset);
    }
}