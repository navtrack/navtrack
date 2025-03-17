import { useAssetsMessagesGetList } from "../../../api";

export type UsePositionsQueryProps = {
  assetId?: string;
  startDate?: string;
  endDate?: string;
  minAltitude?: number;
  maxAltitude?: number;
  minSpeed?: number;
  maxSpeed?: number;
  latitude?: number;
  longitude?: number;
  radius?: number;
};

export function useMessagesQuery(props: UsePositionsQueryProps) {
  const query = useAssetsMessagesGetList(
    props.assetId as string,
    {
      StartDate: props.startDate,
      EndDate: props.endDate,
      MinSpeed: props.minSpeed,
      MaxSpeed: props.maxSpeed,
      MinAltitude: props.minAltitude,
      MaxAltitude: props.maxAltitude,
      Latitude: props.latitude,
      Longitude: props.longitude,
      Radius: props.radius
    },
    {
      query: {
        enabled: !!props.assetId,
        refetchOnWindowFocus: false
      }
    }
  );

  return query;
}
