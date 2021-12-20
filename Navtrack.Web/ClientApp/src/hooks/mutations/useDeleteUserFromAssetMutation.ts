import { useDeleteAssetsAssetIdUsersUserId } from "../../api";

export const useDeleteUserFromAssetMutation = () => {
  const mutation = useDeleteAssetsAssetIdUsersUserId();

  return mutation;
};
