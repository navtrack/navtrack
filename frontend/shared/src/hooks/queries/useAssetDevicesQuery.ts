import { useGetAssetsAssetIdDevices } from "../../api/index-generated";

export const useAssetDevicesQuery = (assetId?: string) => {
  const query = useGetAssetsAssetIdDevices(`${assetId}`, {
    query: {
      refetchOnWindowFocus: false,
      enabled: !!assetId
    }
  });

  return query;
};
