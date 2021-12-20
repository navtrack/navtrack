import { usePostAssetsAssetIdUsers } from "../../api";

export const useAddUserToAssetMutation = () => {
  const mutation = usePostAssetsAssetIdUsers();

  return mutation;
};
