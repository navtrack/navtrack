import { useEffect } from "react";
import { useRouteMatch } from "react-router-dom";
import { useRecoilState, useRecoilValue } from "recoil";
import { currentAssetIdAtom, currentAssetSelector } from "../../state/assets";

export default function useCurrentAsset() {
  const currentAsset = useRecoilValue(currentAssetSelector);
  const [currentAssetId, setCurrentAssetId] =
    useRecoilState(currentAssetIdAtom);
  const match = useRouteMatch<{ id: string }>("/assets/:id/");

  useEffect(() => {
    if (match?.params.id !== currentAssetId) {
      setCurrentAssetId(match?.params.id);
    }
  }, [currentAssetId, match?.params.id, setCurrentAssetId]);

  return { currentAsset };
}
