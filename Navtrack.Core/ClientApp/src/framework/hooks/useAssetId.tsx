import { useParams } from "react-router";

const useAssetId = (): number => {
  let { assetId } = useParams();

  let parsedId = assetId ? parseInt(assetId) : 0;

  return parsedId;
};

export default useAssetId;
