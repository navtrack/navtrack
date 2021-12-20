import { usePostAssets } from "../../api";

export const useAddAssetMutation = () => {
  const mutation = usePostAssets();

  return mutation;
};
