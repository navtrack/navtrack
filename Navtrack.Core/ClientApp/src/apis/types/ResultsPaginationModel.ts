import { PaginationModel } from "./PaginationModel";
import { ResultsModel } from "./ResultsModel";

export interface ResultsPaginationModel<T extends object> extends ResultsModel<T> {
  pagination: PaginationModel;
};
