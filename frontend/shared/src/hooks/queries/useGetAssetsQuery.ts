import { useAssetsGetList } from "../../api/index-generated";

export const useGetAssetsQuery = () => {
  const query = useAssetsGetList({
    query: {
      refetchIntervalInBackground: true,
      refetchInterval: 5000
    }
  });

  return query;
};
