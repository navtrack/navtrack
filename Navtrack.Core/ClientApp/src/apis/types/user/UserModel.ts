import { MeasurementSystem } from "../general/MeasurementSystem";

export type UserModel = {
  id: number;
  email: string;
  measurementSystem: MeasurementSystem;
};

export const DefaultUserModel: UserModel = {
  id: 0,
  email: "",
  measurementSystem: MeasurementSystem.Metric
};
