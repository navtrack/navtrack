import { useCallback } from "react";
import { useCurrentAsset } from "../current/useCurrentAsset";
import { useSetRecoilState } from "recoil";
import { useDeleteAssetMutation } from "../queries/assets/useDeleteAssetMutation";
import { currentAssetIdAtom } from "../../state/current";

type UseDeleteAssetProps = {
  onSuccess: () => void;
};

export function useDeleteAsset(props: UseDeleteAssetProps) {
  const currentAsset = useCurrentAsset();
  const deleteAssetMutation = useDeleteAssetMutation();
  const setCurrentAssetIdAtom = useSetRecoilState(currentAssetIdAtom);

  const handleSubmit = useCallback(
    (assetId: string) => {
      if (currentAsset.data) {
        return deleteAssetMutation.mutateAsync(
          { assetId: assetId },
          {
            onSuccess: () => {
              setCurrentAssetIdAtom(undefined);
              props.onSuccess();
            }
          }
        );
      }

      return Promise.resolve();
    },
    [currentAsset.data, deleteAssetMutation, props, setCurrentAssetIdAtom]
  );

  return {
    handleSubmit,
    isLoading: deleteAssetMutation.isLoading
  };
}
