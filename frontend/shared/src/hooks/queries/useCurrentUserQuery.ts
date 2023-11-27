import { useGetUser } from "../../api/index-generated";

export const useCurrentUserQuery = () => {
  const query = useGetUser({
    query: {
      refetchOnWindowFocus: false,
      refetchOnMount: false
    }
  });

  return query;
};
