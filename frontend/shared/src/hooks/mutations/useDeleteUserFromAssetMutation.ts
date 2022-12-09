import { useDeleteAssetsAssetIdUsersUserId } from "../../api/index-generated";

export const useDeleteUserFromAssetMutation = () => {
  const mutation = useDeleteAssetsAssetIdUsersUserId();

  return mutation;
};
