import { useGetAssetsAssetsAssetIdUsers } from "../../api/index-generated";

type UseAssetUsersQueryProps = {
  assetId: string;
};

export const useAssetUsersQuery = (props: UseAssetUsersQueryProps) => {
  const query = useGetAssetsAssetsAssetIdUsers(props.assetId, {
    query: {
      refetchOnWindowFocus: false
    }
  });

  return query;
};
