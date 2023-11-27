import { MapPin } from "../../ui/map/MapPin";
import { useLiveTracking } from "./useLiveTracking";

export function LiveTrackingMapPin() {
  const { location } = useLiveTracking();

  return (
    <>
      {location && (
        <MapPin latitude={location.latitude} longitude={location.longitude} />
      )}
    </>
  );
}
