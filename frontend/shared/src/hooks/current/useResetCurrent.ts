import { useResetRecoilState } from "recoil";
import {
  currentAssetIdAtom,
  currentOrganizationIdAtom
} from "../../state/current";

export function useResetCurrent() {
  const resetCurrentAssetId = useResetRecoilState(currentAssetIdAtom);
  const resetCurrentOrganizationId = useResetRecoilState(
    currentOrganizationIdAtom
  );

  return () => {
    resetCurrentAssetId();
    resetCurrentOrganizationId();
  };
}
