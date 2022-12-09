import MapPin from "../../ui/shared/map/MapPin";
import useLiveTracking from "./useLiveTracking";

export default function LiveTrackingMapPin() {
  const { location } = useLiveTracking();

  return (
    <>
      {location && (
        <MapPin latitude={location.latitude} longitude={location.longitude} />
      )}
    </>
  );
}
