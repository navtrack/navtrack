import { parseISO } from "date-fns";
import {
  getAssetsTripsGetListQueryKey,
  useAssetsTripsGetList
} from "../../../api";
import { formatApiDate } from "../../../utils/api";

export type AssetTripsQueryProps = {
  assetId?: string;
  date?: string;
  minAltitude?: number;
  maxAltitude?: number;
  minDuration?: number;
  maxDuration?: number;
  minSpeed?: number;
  maxSpeed?: number;
  latitude?: number;
  longitude?: number;
  radius?: number;
};

export function useAssetTripsQuery(props: AssetTripsQueryProps) {
  const query = useAssetsTripsGetList(
    props.assetId!,
    {
      Date: formatApiDate(parseISO(props.date!)),
      MinAvgSpeed: props.minSpeed,
      MaxAvgSpeed: props.maxSpeed,
      MinAltitude: props.minAltitude,
      MaxAltitude: props.maxAltitude,
      MinDuration: props.minDuration,
      MaxDuration: props.maxDuration,
      Latitude: props.latitude,
      Longitude: props.longitude,
      Radius: props.radius
    },
    {
      query: {
        queryKey: getAssetsTripsGetListQueryKey(props.assetId!, {
          Date: props.date,
          MinAvgSpeed: props.minSpeed,
          MaxAvgSpeed: props.maxSpeed,
          MinAltitude: props.minAltitude,
          MaxAltitude: props.maxAltitude,
          MinDuration: props.minDuration,
          MaxDuration: props.maxDuration,
          Latitude: props.latitude,
          Longitude: props.longitude,
          Radius: props.radius
        }),
        enabled: !!props.assetId,
        refetchOnWindowFocus: false
      }
    }
  );

  return query;
}
