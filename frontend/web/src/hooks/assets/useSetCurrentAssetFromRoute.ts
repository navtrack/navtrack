import { currentAssetIdAtom } from "@navtrack/shared/state/assets";
import { useEffect } from "react";
import { useMatch } from "react-router-dom";
import { useRecoilState } from "recoil";
import { Paths } from "../../app/Paths";

export const useSetCurrentAssetFromRoute = () => {
  const [currentAssetId, setCurrentAssetId] =
    useRecoilState(currentAssetIdAtom);
  const match = useMatch(Paths.AssetPattern);

  useEffect(() => {
    if (match?.params.id !== currentAssetId) {
      setCurrentAssetId(match?.params.id);
    }
  }, [currentAssetId, match, setCurrentAssetId]);
};
