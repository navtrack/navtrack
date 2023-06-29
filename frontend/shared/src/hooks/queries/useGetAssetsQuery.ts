import { useGetAssets } from "../../api/index-generated";

export const useGetAssetsQuery = () => {
  const query = useGetAssets({
    query: {
      refetchIntervalInBackground: true,
      refetchInterval: 5000
    }
  });

  return query;
};
