import { AppContextAccessor } from "./AppContext/AppContextAccessor";
import { AssetApi } from "./Api/AssetApi";

export const AssetService = {
  refreshAssets: async () => {
    if (AppContextAccessor.getAppContext().authenticationInfo.authenticated) {
      let assets = await AssetApi.getAll();

      AppContextAccessor.setAppContext({ ...AppContextAccessor.getAppContext(), assets: assets });
    }
  }
};
