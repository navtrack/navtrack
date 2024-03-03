/* eslint-disable react-hooks/rules-of-hooks */
import { appConfigAtom } from "@navtrack/shared/state/appConfig";
import { useInput } from "react-cosmos/client";
import { useSetRecoilState } from "recoil";
import { DEFAULT_MAP_CENTER } from "../../../../constants";
import { Modal } from "../../modal/Modal";
import { Map } from "../Map";
import { GeofenceCircle } from "./GeofenceCircle";

export default {
  Default: () => {
    return (
      <Map center={DEFAULT_MAP_CENTER}>
        <GeofenceCircle />
      </Map>
    );
  },
  "With vector tiles": () => {
    const setAppConfig = useSetRecoilState(appConfigAtom);

    setAppConfig((prev) => ({
      ...prev,
      authentication: {
        clientId: "test"
      },
      api: {
        url: "test"
      },
      map: {
        tileUrl: "https://tiles.stadiamaps.com/styles/osm_bright.json"
      }
    }));

    return (
      <Map center={DEFAULT_MAP_CENTER}>
        <GeofenceCircle />
      </Map>
    );
  },
  "In modal": () => {
    return (
      <Modal open close={() => {}}>
        <div className="flex" style={{ width: 400, height: 400 }}>
          <Map center={DEFAULT_MAP_CENTER}>
            <GeofenceCircle onChange={(geofence) => console.log(geofence)} />
          </Map>
        </div>
      </Modal>
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
    const [geofence, setGeofence] = useInput("geofence", {
      latitude: 46.763266,
      longitude: 23.555374,
      radius: 319
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
