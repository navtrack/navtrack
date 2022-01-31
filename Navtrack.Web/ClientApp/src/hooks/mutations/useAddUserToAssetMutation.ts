import { usePostAssetsAssetIdUsers } from "@navtrack/navtrack-shared";

export const useAddUserToAssetMutation = () => {
  const mutation = usePostAssetsAssetIdUsers();

  return mutation;
};
