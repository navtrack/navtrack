import { getAssetsStatsGetQueryKey, useAssetsStatsGet } from "../../../api";

export type UseAssetStatsQueryProps = {
  assetId?: string;
};

export const useAssetStatsQuery = (props: UseAssetStatsQueryProps) => {
  const query = useAssetsStatsGet(props.assetId as string, {
    query: {
      queryKey: getAssetsStatsGetQueryKey(`${props.assetId}`),
      enabled: !!props.assetId,
      refetchOnWindowFocus: false
    }
  });

  return query;
};
