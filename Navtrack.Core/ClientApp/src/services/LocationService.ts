import { AssetService } from "./AssetService";

let interval: NodeJS.Timeout;

export const LocationService = {
  init: async () => {
    if (!interval) {
      await AssetService.refreshAssets();
      interval = setInterval(AssetService.refreshAssets, 1000 * 10); // TODO replace with signalr
    }
  },

  clear: () => {
    if (interval) {
      clearInterval(interval);
    }
  }
};