import { useContext } from "react";
import { useParams } from "react-router";
import { AssetModel } from "../../apis/types/asset/AssetModel";
import { AppContext } from "../appContext/AppContext";

const useAsset = (): AssetModel | undefined => {
  const { appContext } = useContext(AppContext);
  const { assetId } = useParams<{ assetId: string }>();

  let id = assetId ? parseInt(assetId) : 0;

  if (id > 0 && appContext.assets) {
    return appContext.assets.find((x) => x.id === id);
  }

  return undefined;
};

export default useAsset;
