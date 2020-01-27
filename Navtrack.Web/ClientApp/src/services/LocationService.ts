import { AssetService } from "./AssetService";

let interval: NodeJS.Timeout;

export const LocationService = {
    init: () => {
        if (!interval) {
            AssetService.refreshAssets();
            interval = setInterval(AssetService.refreshAssets, 1000 * 10); // TODO replace with signalr
        }
    },
    
    clear: () => {
        if (interval) {
            clearInterval(interval);
        }
    }
}