import { PaginationModel } from "./PaginationModel";

export type ResultsPaginationModel<T extends object> = {
  results: T[];
  pagination: PaginationModel;
};
