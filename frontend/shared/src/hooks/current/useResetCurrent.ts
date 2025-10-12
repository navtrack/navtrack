import { useSetAtom } from "jotai";
import {
  currentAssetIdAtom,
  currentOrganizationIdAtom
} from "../../state/current";

export function useResetCurrent() {
  const setCurrentAssetId = useSetAtom(currentAssetIdAtom);
  const setCurrentOrganizationId = useSetAtom(currentOrganizationIdAtom);
  return () => {
    setCurrentAssetId(undefined);
    setCurrentOrganizationId(undefined);
  };
}
