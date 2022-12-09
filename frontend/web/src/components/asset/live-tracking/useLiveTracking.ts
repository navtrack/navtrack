import { useCurrentAsset } from "@navtrack/ui-shared/newHooks/assets/useCurrentAsset";
import { assetConfigurationAtom } from "@navtrack/ui-shared/state/assets";
import { useEffect } from "react";
import { useMapEvents } from "react-leaflet";
import { useRecoilState } from "recoil";
import useMap from "../../ui/shared/map/useMap";

export default function useLiveTracking() {
  const currentAsset = useCurrentAsset();
  const [configuration, setConfiguration] = useRecoilState(
    assetConfigurationAtom(currentAsset?.id)
  );
  const { map, setCenter } = useMap();

  useEffect(() => {
    if (configuration.liveTracking.follow && currentAsset?.location) {
      setCenter(
        [currentAsset.location.latitude, currentAsset.location.longitude],
        configuration.liveTracking.zoom
      );
    }
  }, [
    configuration.liveTracking.follow,
    configuration.liveTracking.zoom,
    currentAsset?.location,
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

  return { location: currentAsset?.location };
}
