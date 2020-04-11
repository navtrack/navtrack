import { useState } from "react";
import { useParams } from "react-router";

const useAssetId = (): number | undefined => {
  const [id, setId] = useState<number | undefined>();
  let { assetId } = useParams();

  let parsedId = assetId ? parseInt(assetId) : 0;

  if (parsedId !== id) {
    setId(parsedId > 0 ? parsedId : undefined);
  }

  return id;
};

export default useAssetId;
