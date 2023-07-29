import { usePostAssetsAssetIdUsers } from "../../../api/index-generated";

export function useAddUserToAssetMutation() {
  const mutation = usePostAssetsAssetIdUsers();

  return mutation;
}
