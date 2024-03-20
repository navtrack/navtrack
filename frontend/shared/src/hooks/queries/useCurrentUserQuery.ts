import { useRecoilValue } from "recoil";
import { useUserGet } from "../../api/index-generated";
import { isAuthenticatedAtom } from "../../state/authentication";

export const useCurrentUserQuery = () => {
  const isAuthenticated = useRecoilValue(isAuthenticatedAtom);

  const query = useUserGet({
    query: {
      enabled: isAuthenticated,
      refetchOnWindowFocus: false,
      refetchOnMount: false
    }
  });

  return query;
};
