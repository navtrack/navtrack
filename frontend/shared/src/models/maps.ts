import { LatLongModel } from "../api/model/generated";

export type MapPinUiModel = {
  coordinates?: LatLongModel;
  follow?: boolean;
  color?: "primary" | "green" | "red";
  label?: string;
  assetId?: string;
};
