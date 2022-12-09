import { usePostAssetsAssetIdDevices } from "../../api/index-generated";

export const useAddDeviceMutation = () => {
  const mutation = usePostAssetsAssetIdDevices();

  return mutation;
};
