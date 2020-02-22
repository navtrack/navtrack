import { HttpClient } from "../HttpClient/HttpClient";
import { LocationModel } from "./Model/LocationModel";
import { LocationHistoryRequestModel } from "./Model/LocationHistoryRequestModel";

export const LocationApi = {
    getLatest: function(assetId: number): Promise<LocationModel> {
        return HttpClient.get<LocationModel>(`locations/${assetId}/latest`);
    },

    getHistory: function(filter: LocationHistoryRequestModel): Promise<LocationModel[]> {
        return HttpClient.get<LocationModel[]>("locations/history", filter);
    }
};
