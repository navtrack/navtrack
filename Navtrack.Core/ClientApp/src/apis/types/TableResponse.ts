export type TableResponse<T extends object> = {
  results: T[];
  totalResults: number;
  page: number;
  maxPage: number;
  perPage: number
};