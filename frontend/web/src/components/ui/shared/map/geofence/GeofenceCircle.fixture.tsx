import { GeofenceCircle } from "./GeofenceCircle";
import { Map } from "../Map";
import { DEFAULT_MAP_CENTER } from "../../../../../constants";

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
    return (
      <Map center={DEFAULT_MAP_CENTER}>
        <GeofenceCircle
          geofence={{
            latitude: 46.763266,
            longitude: 23.555374,
            radius: 319
          }}
          onChange={(geofence) => console.log(geofence)}
        />
      </Map>
    );
  }
};
