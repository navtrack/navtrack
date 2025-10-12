import { useCurrentAsset } from "@navtrack/shared/hooks/current/useCurrentAsset";
import { assetConfigurationAtom } from "@navtrack/shared/state/assets";
import { useEffect } from "react";
import { useMapEvents } from "react-leaflet";
import { useAtom } from "jotai";
import { useMap } from "../../ui/map/useMap";

export function useLiveTracking() {
  const currentAsset = useCurrentAsset();
  const [configuration, setConfiguration] = useAtom(
    assetConfigurationAtom(currentAsset.data?.id)
  );
  const map = useMap();

  useEffect(() => {
    if (
      configuration.liveTracking.follow &&
      currentAsset.data?.lastPositionMessage?.position
    ) {
      map.setCenter(
        [
          currentAsset.data.lastPositionMessage?.position.coordinates.latitude,
          currentAsset.data.lastPositionMessage?.position.coordinates.longitude
        ],
        configuration.liveTracking.zoom
      );
    }
  }, [
    configuration.liveTracking.follow,
    configuration.liveTracking.zoom,
    currentAsset.data?.lastPositionMessage?.position,
    map
  ]);

  const mapEvents = useMapEvents({
    zoomend: () => {
      setConfiguration((x) => ({
        ...x,
        liveTracking: { ...x.liveTracking, zoom: mapEvents.getZoom() }
      }));
    }
  });

  return { location: currentAsset.data?.lastPositionMessage?.position };
}
