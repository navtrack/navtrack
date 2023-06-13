import { useGetAssetsAssetsAssetIdUsers } from "../../api/index-generated";

interface IUseAssetUsersQuery {
  assetId: string;
}

export const useAssetUsersQuery = (props: IUseAssetUsersQuery) => {
  const query = useGetAssetsAssetsAssetIdUsers(props.assetId, {
    query: {
      refetchOnWindowFocus: false
    }
  });

  return query;
};
