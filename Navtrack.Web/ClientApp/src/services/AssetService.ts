import {AppContextAccessor} from "./AppContext/AppContextAccessor";
import {AssetApi} from "./Api/AssetApi";

export const AssetService = {
    refreshAssets: async () => {
        const appContext = AppContextAccessor.getAppContext();

        if (appContext.authenticated) {
            await AssetApi.getAll()
                .then(x => AppContextAccessor.setAppContext({...appContext, assets: x}));
        }
    }
};