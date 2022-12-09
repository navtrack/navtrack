import { TripModel } from "../../api/model/generated";

export enum MapEvents {
  LocationPinUpdateEvent,
  TripUpdateEvent
}

export type MapEvent = LocationPinUpdateEvent | TripUpdateEvent;

export type LocationPinUpdateEvent = {
  latitude?: number;
  longitude?: number;
  follow?: boolean;
  resetZoom?: boolean;
};

export type TripUpdateEvent = {
  trip?: TripModel;
};
