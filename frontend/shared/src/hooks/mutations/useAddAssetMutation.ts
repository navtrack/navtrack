import { usePostAssets } from "../../api/index-generated";

export const useAddAssetMutation = () => {
  const mutation = usePostAssets();

  return mutation;
};
