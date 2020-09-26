import { ResultsModel } from "../ResultsModel";
import { TripModel } from "./TripModel";

export interface GetTripsModel extends ResultsModel<TripModel> {
  totalDistance: number;
  totalLocations: number;
}

export const DefaultGetTripsModel: GetTripsModel = {
  totalDistance: 0,
  totalLocations: 0,
  results: []
};
