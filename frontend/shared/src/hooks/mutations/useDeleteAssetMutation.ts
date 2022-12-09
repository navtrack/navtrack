import { useDeleteAssetsAssetId } from "../../api/index-generated";

export const useDeleteAssetMutation = () => {
  const mutation = useDeleteAssetsAssetId();

  return mutation;
};
