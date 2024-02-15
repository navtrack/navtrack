import { TripModel } from "@navtrack/shared/api/model/generated";
import { atom, selector } from "recoil";

export const selectedTripAtom = atom<TripModel | undefined>({
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

    console.log(selectedTrip, selectedTripLocationIndex);

    return selectedTrip !== undefined && selectedTripLocationIndex !== undefined
      ? selectedTrip.positions[selectedTripLocationIndex - 1]
      : undefined;
  }
});
