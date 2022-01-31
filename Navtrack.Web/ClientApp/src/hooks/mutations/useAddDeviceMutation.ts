import { usePostAssetsAssetIdDevices } from "@navtrack/navtrack-shared";

export const useAddDeviceMutation = () => {
  const mutation = usePostAssetsAssetIdDevices();

  return mutation;
};
