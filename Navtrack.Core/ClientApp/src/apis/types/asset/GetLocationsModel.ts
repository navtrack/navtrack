import { LocationModel } from "../location/LocationModel";
import { ResultsModel } from "../ResultsModel";

export interface GetLocationsModel extends ResultsModel<LocationModel> {
}

export const DefaultGetLocationsModel: GetLocationsModel = {
  results: [],
  totalResults: 0
};
