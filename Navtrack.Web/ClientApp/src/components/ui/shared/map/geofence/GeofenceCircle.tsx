import { FormattedMessage } from "react-intl";
import { IGeofenceCircle } from "../types";
import useGeofenceCircle from "./useGeofenceCircle";

export default function GeofenceCircle(props: IGeofenceCircle) {
  const { clicked, geofence } = useGeofenceCircle(props);

  return (
    <div className="z-20 w-full relative p-2 font-sans text-xs flex justify-center">
      <div className="rounded bg-white flex space-x-2 text-left py-1 px-2">
        {clicked ? (
          <>
            <div>
              <span className="font-semibold flex">
                <FormattedMessage id="generic.latitude" />
              </span>
              <span>{geofence?.latitude}</span>
            </div>
            <div>
              <span className="font-semibold flex">
                <FormattedMessage id="generic.longitude" />
              </span>
              <span>{geofence?.longitude}</span>
            </div>
            <div>
              <span className="font-semibold flex">
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
