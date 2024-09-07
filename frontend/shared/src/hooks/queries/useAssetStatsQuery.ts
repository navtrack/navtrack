import { useAssetsStatsGet } from "../../api/index-generated";

export type UseAssetStatsQueryProps = {
  assetId?: string;
};

export const useAssetStatsQuery = (props: UseAssetStatsQueryProps) => {
  const query = useAssetsStatsGet(props.assetId as string, {
    query: {
      enabled: !!props.assetId,
      refetchOnWindowFocus: false
    }
  });

  return query;
};
