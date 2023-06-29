import { useGetAssetsAssetId } from "../../api/index-generated";

type UseGetAssetQueryProps = {
  assetId: string;
};

export function useGetAssetQuery(props: UseGetAssetQueryProps) {
  const query = useGetAssetsAssetId(props.assetId, {
    query: {
      refetchOnWindowFocus: true
    }
  });

  return query;
}
