import { usePostAssets } from "@navtrack/navtrack-shared";

export const useAddAssetMutation = () => {
  const mutation = usePostAssets();

  return mutation;
};
