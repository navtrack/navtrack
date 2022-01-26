import { useGetAssetsAssetIdDevices } from "../../api";

export default function useAssetDevicesQuery(assetId?: string) {
  const query = useGetAssetsAssetIdDevices(`${assetId}`, {
    query: {
      refetchOnWindowFocus: false,
      enabled: !!assetId
    }
  });

  return query;
}
