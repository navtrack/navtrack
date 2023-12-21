import { useAssetsGet } from "../../api/index-generated";

type UseGetAssetQueryProps = {
  assetId: string;
};

export function useGetAssetQuery(props: UseGetAssetQueryProps) {
  const query = useAssetsGet(props.assetId, {
    query: {
      refetchOnWindowFocus: true
    }
  });

  return query;
}
