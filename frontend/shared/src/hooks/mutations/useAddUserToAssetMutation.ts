import { usePostAssetsAssetsAssetIdUsers } from "../../api/index-generated";

export const useAddUserToAssetMutation = () => {
  const mutation = usePostAssetsAssetsAssetIdUsers();

  return mutation;
};
