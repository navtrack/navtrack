import { HttpClient } from "../httpClient/HttpClient";
import { apiUrl, formatUrl } from "services/httpClient/HttpClientUtil";
import { LocationModel } from "./types/location/LocationModel";
import { LocationHistoryRequestModel } from "./types/location/LocationHistoryRequestModel";

export const LocationApi = {
  getLatest: function(assetId: number): Promise<LocationModel> {
    return HttpClient.get<LocationModel>(apiUrl(`locations/${assetId}/latest`));
  },

  getHistory: function(filter: LocationHistoryRequestModel): Promise<LocationModel[]> {
    return HttpClient.get<LocationModel[]>(formatUrl(apiUrl("locations/history"), filter));
  }
};
