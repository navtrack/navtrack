import { useCallback } from "react";
import { useDeleteAssetMutation } from "../../mutations/assets/useDeleteAssetMutation";
import { useCurrentAsset } from "../useCurrentAsset";
import { useSetRecoilState } from "recoil";
import { currentAssetIdAtom } from "../../../state/assets";

type UseDeleteAssetProps = {
  onSuccess: () => void;
};

export function useDeleteAsset(props: UseDeleteAssetProps) {
  const currentAsset = useCurrentAsset();
  const deleteAssetMutation = useDeleteAssetMutation();
  const setSurrentAssetIdAtom = useSetRecoilState(currentAssetIdAtom);

  const handleSubmit = useCallback(
    (assetId: string) => {
      if (currentAsset.data) {
        deleteAssetMutation.mutate(
          { assetId: assetId },
          {
            onSuccess: () => {
              setSurrentAssetIdAtom(undefined);
              props.onSuccess();
            }
          }
        );
      }
    },
    [currentAsset.data, deleteAssetMutation, props, setSurrentAssetIdAtom]
  );

  return {
    handleSubmit,
    isLoading: deleteAssetMutation.isLoading
  };
}
