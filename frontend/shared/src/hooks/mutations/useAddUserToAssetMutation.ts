import { usePostAssetsAssetIdUsers } from "../../api/index-generated";

export const useAddUserToAssetMutation = () => {
  const mutation = usePostAssetsAssetIdUsers();

  return mutation;
};
