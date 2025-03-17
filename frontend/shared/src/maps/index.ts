import { LatLong } from "../api/model";

export type MapPinDto = {
  coordinates: LatLong;
  follow?: boolean;
  color?: "primary" | "green" | "red";
  label?: string;
  assetId?: string;
};

export type MapPaddingDto = {
  top: number;
  left: number;
  right: number;
  bottom: number;
};

export type MapOptionsDto = { padding?: MapPaddingDto; initialZoom?: number };
