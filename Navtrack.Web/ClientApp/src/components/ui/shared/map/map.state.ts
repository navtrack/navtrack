import { Map } from "leaflet";
import { atom } from "recoil";

type Location = {
  latitude: number;
  longitude: number;
};

type MapState = {
  center: Location;
  follow: boolean;
  map?: Map;
};

export const mapAtom = atom<MapState>({
  key: "Map",
  default: {
    center: {
      latitude: 46.770439,
      longitude: 23.591423
    },
    follow: true
  }
});
