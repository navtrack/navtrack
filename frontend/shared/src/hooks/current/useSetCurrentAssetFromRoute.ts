import { useEffect } from "react";
import { useMatch } from "react-router-dom";
import { useRecoilState } from "recoil";
import { currentAssetIdAtom } from "../../state/current";

export function useSetCurrentAssetFromRoute() {
  const [currentAssetId, setCurrentAssetId] =
    useRecoilState(currentAssetIdAtom);
  const match = useMatch("/assets/:id/*");

  useEffect(() => {
    if (match?.params.id !== currentAssetId) {
      setCurrentAssetId(match?.params.id);
    }
  }, [currentAssetId, match, setCurrentAssetId]);
}
