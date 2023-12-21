import { useAssetsDevicesDelete } from "../../../api/index-generated";

export function useDeleteDeviceMutation() {
  const mutation = useAssetsDevicesDelete();

  return mutation;
}
