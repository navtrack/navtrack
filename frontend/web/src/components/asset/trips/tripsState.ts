import { TripModel } from "@navtrack/shared/api/model/generated";
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

export const selectedTripPositionIndexAtom = atom<number | undefined>({
  key: "Trips:SelectedTrip:SelectedPosition:Index",
  default: 1
});

export const selectedTripPositionSelector = selector({
  key: "Trips:SelectedTrip:SelectedPosition",
  get: ({ get }) => {
    const selectedTrip = get(selectedTripSelector);
    const selectedTripLocationIndex = get(selectedTripPositionIndexAtom);

    return selectedTrip !== undefined && selectedTripLocationIndex !== undefined
      ? selectedTrip.positions[selectedTripLocationIndex - 1]
      : undefined;
  }
});
