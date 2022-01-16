import { useGetSettings } from "../../api";

export default function useSettingsQuery() {
  const query = useGetSettings({
    query: {
      refetchOnWindowFocus: false,
      refetchOnMount: false
    }
  });

  return query;
}
