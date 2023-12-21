import { useUserGet } from "../../api/index-generated";

export const useCurrentUserQuery = () => {
  const query = useUserGet({
    query: {
      refetchOnWindowFocus: false,
      refetchOnMount: false
    }
  });

  return query;
};
