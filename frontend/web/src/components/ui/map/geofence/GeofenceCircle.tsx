import { FormattedMessage } from "react-intl";
import { IGeofenceCircle } from "../types";
import { useGeofenceCircle } from "./useGeofenceCircle";

export function GeofenceCircle(props: IGeofenceCircle) {
  const { clicked, geofence } = useGeofenceCircle(props);

  return (
    <div className="relative z-20 flex w-full justify-center p-2 font-sans text-xs">
      <div className="flex space-x-2 rounded bg-white py-1 px-2 text-left">
        {clicked ? (
          <>
            <div>
              <span className="flex font-semibold">
                <FormattedMessage id="generic.latitude" />
              </span>
              <span>{geofence?.latitude}</span>
            </div>
            <div>
              <span className="flex font-semibold">
                <FormattedMessage id="generic.longitude" />
              </span>
              <span>{geofence?.longitude}</span>
            </div>
            <div>
              <span className="flex font-semibold">
                <FormattedMessage id="generic.radius" />
              </span>
              <span>{geofence?.radius} m</span>
            </div>
          </>
        ) : (
          <FormattedMessage id="locations.filter.geofence.select-center" />
        )}
      </div>
    </div>
  );
}
