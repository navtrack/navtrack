import { useDeleteAssetsAssetId } from "@navtrack/navtrack-shared";

export const useDeleteAssetMutation = () => {
  const mutation = useDeleteAssetsAssetId();

  return mutation;
};
