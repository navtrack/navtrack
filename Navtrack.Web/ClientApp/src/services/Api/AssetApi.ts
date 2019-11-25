import { HttpClient } from "../HttpClient/HttpClient";
import { AssetModel } from "./Model/AssetModel";

export const AssetApi = {
    get: function(id: number) : Promise<AssetModel> {
        return HttpClient.get<AssetModel>("assets/" + id);
    },

    getAll: function() : Promise<AssetModel[]> {
        return HttpClient.get<AssetModel[]>("assets");
    },

    update: function(asset: AssetModel) : Promise<Response> {
        return HttpClient.put("assets/" + asset.id, asset);
    },
    
    add: function(asset: AssetModel) : Promise<Response> {
        return HttpClient.post("assets", asset);
    }
}