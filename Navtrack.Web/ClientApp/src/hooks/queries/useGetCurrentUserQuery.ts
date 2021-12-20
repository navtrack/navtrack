import { useGetUser } from "../../api";

export default function useGetCurrentUserQuery() {
  const query = useGetUser({
    query: {
      refetchOnWindowFocus: false,
      refetchOnMount: false
    }
  });

  return query;
}
