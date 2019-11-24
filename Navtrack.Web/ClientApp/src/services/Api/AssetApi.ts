import { Api } from "./Api";
import { Asset } from "./Types/Asset";

export const AssetApi = {
    get: function(id: number) : Promise<Asset> {
        return Api.get<Asset>("assets/" + id);
    },

    getAll: function() : Promise<Asset[]> {
        return Api.get<Asset[]>("assets");
    },

    update: function(asset: Asset) : Promise<Response> {
        return Api.put("assets/" + asset.id, asset);
    },
    
    add: function(asset: Asset) : Promise<Response> {
        return Api.post("assets", asset);
    }
}