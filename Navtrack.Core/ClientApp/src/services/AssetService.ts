import { AssetsHub } from "../apis/signalR/AssetsHub";
import { AppContextAccessor } from "./appContext/AppContextAccessor";

let interval: NodeJS.Timeout;

export const AssetsService = {
  init: async () => {
    if (!interval) {
      await AssetsService.refreshAssets();

      interval = setInterval(AssetsService.refreshAssets, 1000 * 10);
    }
  },

  clear: () => {
    if (interval) {
      clearInterval(interval);
    }
  },

  refreshAssets: async () => {
    let assets = await AssetsHub.getAll();

    AppContextAccessor.setAppContext((x) => {
      return { ...x, assets: assets };
    });
  }
};
