import { DEFAULT_MAP_CENTER } from "../../../../constants";
import { Map } from "./Map";
import MapPin from "./MapPin";

export default {
  Default: () => {
    return <Map center={DEFAULT_MAP_CENTER} />;
  },
  WithPin: () => {
    return (
      <Map center={DEFAULT_MAP_CENTER}>
        <MapPin latitude={46.770439} longitude={23.591423} />
      </Map>
    );
  }
};
