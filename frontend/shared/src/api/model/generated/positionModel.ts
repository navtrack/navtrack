/**
 * Generated by orval v6.24.0 🍺
 * Do not edit manually.
 * Navtrack.Api
 * OpenAPI spec version: 1.0.0
 */
import type { PositionModelGsm } from "./positionModelGsm";

export interface PositionModel {
  altitude?: number | null;
  coordinates: number[];
  date: string;
  gsm?: PositionModelGsm;
  hdop?: number | null;
  heading?: number | null;
  id?: string;
  latitude: number;
  longitude: number;
  odometer?: number | null;
  satellites?: number | null;
  speed?: number | null;
  valid?: boolean | null;
  validCoordinates: boolean;
}
