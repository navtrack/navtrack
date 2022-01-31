import { usePatchAssetsAssetId } from "@navtrack/navtrack-shared";

export const useRenameAssetMutation = () => {
  const mutation = usePatchAssetsAssetId();

  return mutation;
};
