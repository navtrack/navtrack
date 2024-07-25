import { useFixtureInput } from "react-cosmos/client";
import { Map } from "./Map";
import { MapCustomMarker } from "./MapCustomMarker";

export default {
  Button: () => {
    const [latitude] = useFixtureInput("latitude", 46.770225);
    const [longitude] = useFixtureInput("longitude", 23.588352);
    const [visible] = useFixtureInput("visible", true);

    return (
      <Map center={{ latitude, longitude }} initialZoom={20}>
        {visible && (
          <MapCustomMarker
            coordinates={{ latitude: latitude, longitude: longitude }}
            follow>
            <button
              className="w-20 rounded border bg-red-100 p-2"
              onClick={() => alert("clicked")}>
              Click me
            </button>
          </MapCustomMarker>
        )}
      </Map>
    );
  }
};
