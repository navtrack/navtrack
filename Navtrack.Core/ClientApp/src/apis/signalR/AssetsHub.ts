import { apiUrl } from "../../services/httpClient/HttpClientUtil";
import { SignalRClient } from "../../services/signalR/SignalRClient";
import { AssetModel } from "../types/asset/AssetModel";

export const AssetsHub = {
  getAll: async (): Promise<AssetModel[]> => {
    let assets = await SignalRClient.invoke<AssetModel[]>(apiUrl("hubs/assets"), "GetAll");

    return assets;
  }
};
