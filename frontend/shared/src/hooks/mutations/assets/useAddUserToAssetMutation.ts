import { usePostAssetsAssetsAssetIdUsers } from "../../../api/index-generated";

export function useAddUserToAssetMutation() {
  const mutation = usePostAssetsAssetsAssetIdUsers();

  return mutation;
}
