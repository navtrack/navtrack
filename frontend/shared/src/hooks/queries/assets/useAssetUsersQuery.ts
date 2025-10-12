import {
  getAssetsUsersGetListQueryKey,
  useAssetsUsersGetList
} from "../../../api";

type UseAssetUsersQueryProps = {
  assetId: string;
};

export const useAssetUsersQuery = (props: UseAssetUsersQueryProps) => {
  const query = useAssetsUsersGetList(props.assetId, {
    query: {
      queryKey: getAssetsUsersGetListQueryKey(`${props.assetId}`),
      refetchOnWindowFocus: false
    }
  });

  return query;
};
