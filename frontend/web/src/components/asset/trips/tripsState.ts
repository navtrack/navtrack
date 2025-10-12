import { Trip } from "@navtrack/shared/api/model";
import { atom } from "jotai";

export const selectedTripAtom = atom<Trip | undefined>(undefined);

export const selectedTripPositionIndexAtom = atom<number | undefined>(1);

export const selectedTripPositionSelector = atom((get) => {
  const selectedTrip = get(selectedTripAtom);
  const selectedTripLocationIndex = get(selectedTripPositionIndexAtom);

  return selectedTrip !== undefined && selectedTripLocationIndex !== undefined
    ? selectedTrip.positions[selectedTripLocationIndex - 1]
    : undefined;
});
