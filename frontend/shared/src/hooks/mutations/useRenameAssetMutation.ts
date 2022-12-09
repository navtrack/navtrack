import { usePatchAssetsAssetId } from "../../api/index-generated";

export const useRenameAssetMutation = () => {
  const mutation = usePatchAssetsAssetId();

  return mutation;
};
