import { useGetAssetsAssetIdUsers } from "../../api/index-generated";

type UseAssetUsersQueryProps = {
  assetId: string;
};

export const useAssetUsersQuery = (props: UseAssetUsersQueryProps) => {
  const query = useGetAssetsAssetIdUsers(props.assetId, {
    query: {
      refetchOnWindowFocus: false
    }
  });

  return query;
};
