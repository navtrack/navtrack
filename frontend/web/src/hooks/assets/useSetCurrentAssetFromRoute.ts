import { currentAssetIdAtom } from "@navtrack/shared/state/assets";
import { useEffect } from "react";
import { useRouteMatch } from "react-router-dom";
import { useRecoilState } from "recoil";

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
