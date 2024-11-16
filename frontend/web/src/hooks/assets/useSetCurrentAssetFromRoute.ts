import { currentAssetIdAtom } from "@navtrack/shared/state/current";
import { useEffect } from "react";
import { useMatch } from "react-router-dom";
import { useRecoilState } from "recoil";

export const useSetCurrentAssetFromRoute = () => {
  const [currentAssetId, setCurrentAssetId] =
    useRecoilState(currentAssetIdAtom);
  const match = useMatch("/assets/:id/*");

  useEffect(() => {
    if (match?.params.id !== currentAssetId) {
      setCurrentAssetId(match?.params.id);
    }
  }, [currentAssetId, match, setCurrentAssetId]);
};
