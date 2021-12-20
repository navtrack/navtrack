import { useDeleteAssetsAssetId } from "../../api";

export const useDeleteAssetMutation = () => {
  const mutation = useDeleteAssetsAssetId();

  return mutation;
};
