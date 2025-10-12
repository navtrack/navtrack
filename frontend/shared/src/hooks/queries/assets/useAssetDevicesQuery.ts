import {
  getAssetsDevicesGetListQueryKey,
  useAssetsDevicesGetList
} from "../../../api";

export const useAssetDevicesQuery = (assetId?: string) => {
  const query = useAssetsDevicesGetList(`${assetId}`, {
    query: {
      queryKey: getAssetsDevicesGetListQueryKey(`${assetId}`),
      refetchOnWindowFocus: false,
      enabled: !!assetId
    }
  });

  return query;
};
