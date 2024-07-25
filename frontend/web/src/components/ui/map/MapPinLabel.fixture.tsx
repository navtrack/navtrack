import { useFixtureInput } from "react-cosmos/client";
import { Map } from "./Map";
import { MapPinLabel } from "./MapPinLabel";

export default {
  Default: () => {
    const [latitude] = useFixtureInput("latitude", 46.770225);
    const [longitude] = useFixtureInput("longitude", 23.588352);
    const [visible] = useFixtureInput("visible", true);

    return (
      <Map center={{ latitude, longitude }} initialZoom={20}>
        {visible && (
          <MapPinLabel
            coordinates={{ latitude, longitude }}
            label="Choco's car"
          />
        )}
      </Map>
    );
  }
};
