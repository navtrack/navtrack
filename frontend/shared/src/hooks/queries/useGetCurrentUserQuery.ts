import { useGetUser } from "../../api/index-generated";

export const useGetCurrentUserQuery = () => {
  const query = useGetUser({
    query: {
      refetchOnWindowFocus: false,
      refetchOnMount: false,
    },
  });

  return query;
};
