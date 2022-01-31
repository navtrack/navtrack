import { useGetSettings } from "@navtrack/navtrack-shared";

export default function useSettingsQuery() {
  const query = useGetSettings({
    query: {
      refetchOnWindowFocus: false,
      refetchOnMount: false
    }
  });

  return query;
}
