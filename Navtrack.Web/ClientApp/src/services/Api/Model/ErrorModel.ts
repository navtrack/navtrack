export type ErrorModel = {
  status: number,
  title: string,
  errors: { [id: string] : string[]; }
};