import { useAssetsUsersDelete } from "../../../api/index-generated";

export function useAssetUserDeleteMutation() {
  const mutation = useAssetsUsersDelete();

  return mutation;
}
