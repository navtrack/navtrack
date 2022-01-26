import { usePostAssetsAssetIdDevices } from "../../api";

export const useAddDeviceMutation = () => {
  const mutation = usePostAssetsAssetIdDevices();

  return mutation;
};
