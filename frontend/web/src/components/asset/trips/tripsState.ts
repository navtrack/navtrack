import { Trip } from "@navtrack/shared/api/model";
import { atom, selector } from "recoil";

export const selectedTripAtom = atom<Trip | undefined>({
  key: "Trips:SelectedTrip",
  default: undefined
});

export const selectedTripPositionIndexAtom = atom<number | undefined>({
  key: "Trips:SelectedTrip:SelectedPosition:Index",
  default: 1
});

export const selectedTripPositionSelector = selector({
  key: "Trips:SelectedTrip:SelectedPosition",
  get: ({ get }) => {
    const selectedTrip = get(selectedTripAtom);
    const selectedTripLocationIndex = get(selectedTripPositionIndexAtom);

    return selectedTrip !== undefined && selectedTripLocationIndex !== undefined
      ? selectedTrip.positions[selectedTripLocationIndex - 1]
      : undefined;
  }
});
