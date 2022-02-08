import { TripModel } from "@navtrack/navtrack-app-shared/dist/api/model/generated";
import { atom, selector } from "recoil";

export const tripsAtom = atom<TripModel[]>({
  key: "Trips",
  default: []
});

export const selectedTripIndexAtom = atom<number | undefined>({
  key: "Trips:SelectedTrip:Index",
  default: undefined
});

export const selectedTripSelector = selector({
  key: "Trips:SelectedTrip",
  get: ({ get }) => {
    const trips = get(tripsAtom);
    const selectedTripIndex = get(selectedTripIndexAtom);

    return selectedTripIndex !== undefined
      ? trips[selectedTripIndex]
      : undefined;
  }
});

export const selectedTripLocationIndexAtom = atom<number | undefined>({
  key: "Trips:SelectedTrip:SelectedLocation:Index",
  default: 1
});

export const selectedTripLocationSelector = selector({
  key: "Trips:SelectedTrip:SelectedLocation",
  get: ({ get }) => {
    const selectedTrip = get(selectedTripSelector);
    const selectedTripLocationIndex = get(selectedTripLocationIndexAtom);

    return selectedTrip !== undefined && selectedTripLocationIndex !== undefined
      ? selectedTrip.locations[selectedTripLocationIndex - 1]
      : undefined;
  }
});
