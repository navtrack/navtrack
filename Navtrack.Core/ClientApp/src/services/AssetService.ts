import { AppContextAccessor } from "./appContext/AppContextAccessor";
import { AssetApi } from "./api/AssetApi";

export const AssetService = {
  refreshAssets: async () => {
    if (AppContextAccessor.getAppContext().authenticationInfo.authenticated) {
      let assets = await AssetApi.getAll();

      AppContextAccessor.setAppContext({ ...AppContextAccessor.getAppContext(), assets: assets });
    }
  }
};
