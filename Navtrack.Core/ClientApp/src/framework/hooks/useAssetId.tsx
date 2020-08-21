import { useParams } from "react-router";

const useAssetId = (): number => {
  let { assetId } = useParams<{ assetId: string }>();

  return assetId ? parseInt(assetId) : 0;
};

export default useAssetId;
