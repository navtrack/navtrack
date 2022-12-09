import { useGetAssetsAssetIdUsers } from "../../api/index-generated";

interface IUseAssetUsersQuery {
  assetId: string;
}

export const useAssetUsersQuery = (props: IUseAssetUsersQuery) => {
  const query = useGetAssetsAssetIdUsers(props.assetId, {
    query: {
      refetchOnWindowFocus: false
    }
  });

  return query;
};
