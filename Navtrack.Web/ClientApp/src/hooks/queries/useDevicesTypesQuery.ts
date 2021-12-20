import { useGetDevicesTypes } from "../../api";

export default function useDevicesTypesQuery() {
  const query = useGetDevicesTypes({
    query: { refetchOnMount: false, refetchOnWindowFocus: false }
  });

  return query;
}
