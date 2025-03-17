import { useDevicesGetList } from "../../../api";

export const useDevicesTypesQuery = () => {
  const query = useDevicesGetList({
    query: { refetchOnMount: false, refetchOnWindowFocus: false }
  });

  return query;
};
