import { AppContextAccessor } from "framework/appContext/AppContextAccessor";
import { AssetApi } from "apis/AssetApi";

export const AssetService = {
  refreshAssets: async () => {
    if (AppContextAccessor.getAppContext().authenticationInfo.authenticated) {
      let assets = await AssetApi.getAll();

      AppContextAccessor.setAppContext({ ...AppContextAccessor.getAppContext(), assets: assets });
    }
  }
};
