/**
 * Generated by orval v6.24.0 🍺
 * Do not edit manually.
 * Navtrack.Api
 * OpenAPI spec version: 1.0.0
 */
import type { AssetStatsDateRange } from "./assetStatsDateRange";

export interface AssetStatsItemModel {
  dateRange: AssetStatsDateRange;
  distance?: number | null;
  distanceChange?: number | null;
  distancePrevious?: number | null;
  duration?: number | null;
  durationChange?: number | null;
  durationPrevious?: number | null;
  fuelConsumption?: number | null;
  fuelConsumptionChange?: number | null;
  fuelConsumptionPrevious?: number | null;
}
