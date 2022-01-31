import { useDeleteAssetsAssetIdUsersUserId } from "@navtrack/navtrack-shared";

export const useDeleteUserFromAssetMutation = () => {
  const mutation = useDeleteAssetsAssetIdUsersUserId();

  return mutation;
};
