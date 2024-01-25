import { useCurrentAsset } from "@navtrack/shared/hooks/assets/useCurrentAsset";
import { assetConfigurationAtom } from "@navtrack/shared/state/assets";
import { useEffect } from "react";
import { useMapEvents } from "react-leaflet";
import { useRecoilState } from "recoil";
import { useMap } from "../../ui/map/useMap";

export function useLiveTracking() {
  const currentAsset = useCurrentAsset();
  const [configuration, setConfiguration] = useRecoilState(
    assetConfigurationAtom(currentAsset.data?.id)
  );
  const { map, setCenter } = useMap();

  useEffect(() => {
    if (configuration.liveTracking.follow && currentAsset.data?.position) {
      setCenter(
        [
          currentAsset.data.position.latitude,
          currentAsset.data.position.longitude
        ],
        configuration.liveTracking.zoom
      );
    }
  }, [
    configuration.liveTracking.follow,
    configuration.liveTracking.zoom,
    currentAsset.data?.position,
    map,
    setCenter
  ]);

  const mapEvents = useMapEvents({
    zoomend: () => {
      setConfiguration((x) => ({
        ...x,
        liveTracking: { ...x.liveTracking, zoom: mapEvents.getZoom() }
      }));
    }
  });

  return { location: currentAsset.data?.position };
}
