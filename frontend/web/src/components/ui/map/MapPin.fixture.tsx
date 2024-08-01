import { useFixtureInput } from "react-cosmos/client";
import { Map } from "./Map";
import { MapPin } from "./MapPin";

export default {
  Default: () => {
    const [latitude] = useFixtureInput("latitude", 46.770225);
    const [longitude] = useFixtureInput("longitude", 23.588352);
    const [visible] = useFixtureInput("visible", true);

    return (
      <Map center={{ latitude, longitude }} initialZoom={20}>
        {visible && <MapPin pin={{ coordinates: { latitude, longitude } }} />}
      </Map>
    );
  }
};
