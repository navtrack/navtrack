import { useCurrentAsset } from "./useCurrentAsset";
import { useCurrentOrganization } from "./useCurrentOrganization";

export function useResetCurrent() {
  const currentAsset = useCurrentAsset();
  const currentOrganization = useCurrentOrganization();

  return () => {
    currentAsset.reset();
    currentOrganization.reset();
  };
}
