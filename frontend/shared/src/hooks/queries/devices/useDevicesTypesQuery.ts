import { useDevicesGetList } from "../../../api/index-generated";

export const useDevicesTypesQuery = () => {
  const query = useDevicesGetList({
    query: { refetchOnMount: false, refetchOnWindowFocus: false }
  });

  return query;
};
