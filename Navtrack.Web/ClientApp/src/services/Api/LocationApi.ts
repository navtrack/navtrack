import { HttpClient } from "../HttpClient/HttpClient";
import { LocationModel } from "./Model/LocationModel";

export const LocationApi = {
    getLatest: function(assetId: number) : Promise<LocationModel> {
        return HttpClient.get<LocationModel>(`locations/${assetId}/latest`);
    },
    
    getHistory: function(assetId: number) : Promise<LocationModel[]> {
        return HttpClient.get<LocationModel[]>(`locations/${assetId}/history`);
    }
}