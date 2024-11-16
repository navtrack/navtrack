import { useAssetsUsersGetList } from "../../../api/index-generated";

type UseAssetUsersQueryProps = {
  assetId: string;
};

export const useAssetUsersQuery = (props: UseAssetUsersQueryProps) => {
  const query = useAssetsUsersGetList(props.assetId, {
    query: {
      refetchOnWindowFocus: false
    }
  });

  return query;
};
