import { useCurrentOrganization } from "../current/useCurrentOrganization";
import { useAuthentication } from "./authentication/useAuthentication";
import { useCurrentAsset } from "../current/useCurrentAsset";

export function useAppState() {
  const authentication = useAuthentication();
  const currentOrganization = useCurrentOrganization();
  const currentAsset = useCurrentAsset();

  return {
    initialized:
      authentication.state.initialized &&
      currentOrganization.initialized &&
      currentAsset.initialized
  };
}
