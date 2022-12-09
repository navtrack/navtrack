import { useGetAssetsSignalRQuery } from "../../hooks/queries/useGetAssetsSignalRQuery";

export const useGetAssets = () => {
  const query = useGetAssetsSignalRQuery();

  return query.data?.items ?? [];
};
