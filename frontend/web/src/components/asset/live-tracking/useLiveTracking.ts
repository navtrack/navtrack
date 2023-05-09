import { useCurrentAsset } from "@navtrack/shared/hooks/assets/useCurrentAsset";
import { assetConfigurationAtom } from "@navtrack/shared/state/assets";
import { useEffect } from "react";
import { useMapEvents } from "react-leaflet";
import { useRecoilState } from "recoil";
import { useMap } from "../../ui/shared/map/useMap";

export function useLiveTracking() {
  const currentAsset = useCurrentAsset();
  const [configuration, setConfiguration] = useRecoilState(
    assetConfigurationAtom(currentAsset.data?.id)
  );
  const { map, setCenter } = useMap();

  useEffect(() => {
    if (configuration.liveTracking.follow && currentAsset.data?.location) {
      setCenter(
        [
          currentAsset.data.location.latitude,
          currentAsset.data.location.longitude
        ],
        configuration.liveTracking.zoom
      );
    }
  }, [
    configuration.liveTracking.follow,
    configuration.liveTracking.zoom,
    currentAsset.data?.location,
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

  return { location: currentAsset.data?.location };
}
