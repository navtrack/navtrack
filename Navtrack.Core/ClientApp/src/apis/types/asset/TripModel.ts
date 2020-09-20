import { LocationModel } from "../location/LocationModel";

export type TripModel = {
  number: number;
  startDate: Date;
  endDate: Date;
  locations: LocationModel[];
  distance: number;
};
