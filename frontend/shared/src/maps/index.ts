import { LatLong } from "../api/model/generated";

export type FeMapPin = {
  coordinates?: LatLong;
  follow?: boolean;
  color?: "primary" | "green" | "red";
  label?: string;
  assetId?: string;
};

export type MapPadding = {
  top: number;
  left: number;
  right: number;
  bottom: number;
};

export type MapOptions = {
  padding?: MapPadding;
};
