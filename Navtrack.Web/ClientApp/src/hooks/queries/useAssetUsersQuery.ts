import { useGetAssetsAssetIdUsers } from "../../api";

interface IUseAssetUsersQuery {
  assetId: string;
}

export default function useAssetUsersQuery(props: IUseAssetUsersQuery) {
  const query = useGetAssetsAssetIdUsers(props.assetId, {
    query: {
      refetchOnWindowFocus: false
    }
  });

  return query;
}
