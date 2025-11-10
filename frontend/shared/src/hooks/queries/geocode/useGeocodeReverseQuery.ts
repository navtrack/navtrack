import { getGeocodeReverseQueryKey, useGeocodeReverse } from "../../../api";
import { GeocodeReverseParams } from "../../../api/model";

type GeocodeReverseQueryProps = GeocodeReverseParams & { isEnabled?: boolean };

export function useGeocodeReverseQuery(props: GeocodeReverseQueryProps) {
  const query = useGeocodeReverse(props, {
    query: {
      queryKey: getGeocodeReverseQueryKey({ lat: props.lat, lon: props.lon }),
      refetchOnWindowFocus: false,
      refetchOnMount: false,
      staleTime: Infinity,
      enabled:
        (props.isEnabled || props.isEnabled === undefined) &&
        props.lat !== undefined &&
        props.lon !== undefined &&
        props.lat !== 0 &&
        props.lon !== 0
    }
  });

  return query;
}
