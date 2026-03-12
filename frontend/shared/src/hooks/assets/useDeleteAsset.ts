import { useCallback } from "react";
import { useCurrentAsset } from "../current/useCurrentAsset";
import { useDeleteAssetMutation } from "../queries/assets/useDeleteAssetMutation";

type UseDeleteAssetProps = {
  onSuccess: () => void;
};

export function useDeleteAsset(props: UseDeleteAssetProps) {
  const currentAsset = useCurrentAsset();
  const deleteAssetMutation = useDeleteAssetMutation();

  const handleSubmit = useCallback(
    (assetId: string) => {
      if (currentAsset.data) {
        return deleteAssetMutation.mutateAsync(
          { assetId: assetId },
          {
            onSuccess: () => {
              props.onSuccess();
            }
          }
        );
      }

      return Promise.resolve();
    },
    [currentAsset.data, deleteAssetMutation, props]
  );

  return {
    handleSubmit,
    isLoading: deleteAssetMutation.isPending
  };
}
