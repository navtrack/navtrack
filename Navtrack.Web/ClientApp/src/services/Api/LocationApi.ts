import { HttpClient } from "../HttpClient/HttpClient";
import { LocationModel } from "./Model/LocationModel";
import { LocationHistoryRequestModel } from "./Model/LocationHistoryRequestModel";
import { apiUrl } from "services/HttpClient/HttpClientUtil";

export const LocationApi = {
  getLatest: function(assetId: number): Promise<LocationModel> {
    return HttpClient.get<LocationModel>(apiUrl(`locations/${assetId}/latest`));
  },

  getHistory: function(filter: LocationHistoryRequestModel): Promise<LocationModel[]> {
    return HttpClient.get<LocationModel[]>(apiUrl("locations/history"), filter);
  }
};
