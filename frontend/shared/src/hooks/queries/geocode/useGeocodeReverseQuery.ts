import { useGeocodeReverse } from "../../../api";
import { GeocodeReverseParams } from "../../../api/model";

export function useGeocodeReverseQuery(props: GeocodeReverseParams) {
  const query = useGeocodeReverse(props, {
    query: {
      refetchOnWindowFocus: false,
      refetchOnMount: false,
      enabled:
        props.lat !== undefined &&
        props.lon !== undefined &&
        props.lat !== 0 &&
        props.lon !== 0
    }
  });

  return query;
}
