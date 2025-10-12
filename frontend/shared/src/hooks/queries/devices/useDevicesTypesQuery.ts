import { getDevicesGetListQueryKey, useDevicesGetList } from "../../../api";

export const useDevicesTypesQuery = () => {
  const query = useDevicesGetList({
    query: {
      queryKey: getDevicesGetListQueryKey(),
      refetchOnMount: false,
      refetchOnWindowFocus: false
    }
  });

  return query;
};
