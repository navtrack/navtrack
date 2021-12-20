import { usePatchAssetsAssetId } from "../../api";

export const useRenameAssetMutation = () => {
  const mutation = usePatchAssetsAssetId();

  return mutation;
};
