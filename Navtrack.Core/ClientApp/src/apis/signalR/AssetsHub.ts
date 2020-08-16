import { AssetModel } from "apis/types/asset/AssetModel";
import { SignalRClient } from "framework/signalR/SignalRClient";
import { apiUrl } from "framework/httpClient/HttpClientUtil";

export const AssetsHub = {
  getAll: async (): Promise<AssetModel[]> => {
    let assets = await SignalRClient.invoke<AssetModel[]>(apiUrl("hubs/assets"), "GetAll");

    return assets;
  }
};
