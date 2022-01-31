import { useGetUser } from "@navtrack/navtrack-shared";

export default function useGetCurrentUserQuery() {
  const query = useGetUser({
    query: {
      refetchOnWindowFocus: false,
      refetchOnMount: false
    }
  });

  return query;
}
