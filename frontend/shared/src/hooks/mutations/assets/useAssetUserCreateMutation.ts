import { useAssetsUsersCreate } from "../../../api/index-generated";

export function useAssetUserCreateMutation() {
  const mutation = useAssetsUsersCreate();

  return mutation;
}
