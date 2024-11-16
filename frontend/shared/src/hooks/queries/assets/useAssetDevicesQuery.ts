import { useAssetsDevicesGetList } from "../../../api/index-generated";

export const useAssetDevicesQuery = (assetId?: string) => {
  const query = useAssetsDevicesGetList(`${assetId}`, {
    query: {
      refetchOnWindowFocus: false,
      enabled: !!assetId
    }
  });

  return query;
};
