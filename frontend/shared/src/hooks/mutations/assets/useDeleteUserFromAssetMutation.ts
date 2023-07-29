import { useDeleteAssetsAssetIdUsersUserId } from "../../../api/index-generated";

export function useDeleteUserFromAssetMutation() {
  const mutation = useDeleteAssetsAssetIdUsersUserId();

  return mutation;
}
