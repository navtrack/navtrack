import { useValue } from "react-cosmos/client";
import { DEFAULT_MAP_CENTER } from "../../../../constants";
import { Map } from "../Map";
import { CircleGeofence } from "../mapTypes";
import { GeofenceCircle } from "./GeofenceCircle";

export default {
  Default: () => {
    return (
      <Map center={DEFAULT_MAP_CENTER}>
        <GeofenceCircle />
      </Map>
    );
  },
  "With event handler": () => {
    return (
      <Map center={DEFAULT_MAP_CENTER}>
        <GeofenceCircle onChange={(geofence) => console.log(geofence)} />
      </Map>
    );
  },
  "With geofence": () => {
    const [geofence, setGeofence] = useValue<CircleGeofence>("geofence", {
      defaultValue: {
        latitude: 46.763266,
        longitude: 23.555374,
        radius: 319
      }
    });
    return (
      <Map center={DEFAULT_MAP_CENTER}>
        <GeofenceCircle
          geofence={geofence}
          onChange={(geofence) => setGeofence(geofence)}
        />
      </Map>
    );
  }
};
