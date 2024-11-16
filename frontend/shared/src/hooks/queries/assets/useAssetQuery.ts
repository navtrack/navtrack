import { useAssetsGet } from "../../../api/index-generated";

type UseAssetQueryProps = {
  assetId?: string;
};

export function useAssetQuery(props: UseAssetQueryProps) {
  const query = useAssetsGet(props.assetId!, {
    query: {
      refetchOnWindowFocus: true,
      enabled: props.assetId !== undefined
    }
  });

  return query;
}
