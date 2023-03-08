import { useEffect } from "react";
import { useRouteMatch } from "react-router-dom";
import { useRecoilState } from "recoil";
import { currentAssetIdAtom } from "../../state/assets";

export const useSetCurrentAssetFromRoute = () => {
  const [currentAssetId, setCurrentAssetId] =
    useRecoilState(currentAssetIdAtom);
  const match = useRouteMatch<{ id: string }>("/assets/:id/");

  useEffect(() => {
    if (match?.params.id !== currentAssetId) {
      setCurrentAssetId(match?.params.id);
    }
  }, [currentAssetId, match?.params.id, setCurrentAssetId]);
};
