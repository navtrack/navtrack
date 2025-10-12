import { getUserGetQueryKey, useUserGet } from "../../../api";
import { useAuthentication } from "../../app/authentication/useAuthentication";

export const useCurrentUserQuery = () => {
  const authentication = useAuthentication();

  const query = useUserGet({
    query: {
      queryKey: getUserGetQueryKey(),
      enabled: authentication.isAuthenticated,
      refetchOnWindowFocus: false,
      refetchOnMount: false
    }
  });

  return query;
};
