import { useGetSettings } from "../../api/index-generated";

export const useSettingsQuery = () => {
  const query = useGetSettings({
    query: {
      refetchOnWindowFocus: false,
      refetchOnMount: false
    }
  });

  return query;
};
