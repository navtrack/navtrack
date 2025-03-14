/**
 * Generated by orval v6.31.0 🍺
 * Do not edit manually.
 * Navtrack.Api
 * OpenAPI spec version: 1.0.0
 */
import type { AssetStatsDateRange } from "./assetStatsDateRange";

export interface AssetStatItem {
  dateRange: AssetStatsDateRange;
  /** @nullable */
  distance?: number | null;
  /** @nullable */
  distanceChange?: number | null;
  /** @nullable */
  distancePrevious?: number | null;
  /** @nullable */
  duration?: number | null;
  /** @nullable */
  durationChange?: number | null;
  /** @nullable */
  durationPrevious?: number | null;
  /** @nullable */
  fuelConsumption?: number | null;
  /** @nullable */
  fuelConsumptionAverage?: number | null;
  /** @nullable */
  fuelConsumptionAverageChange?: number | null;
  /** @nullable */
  fuelConsumptionAveragePrevious?: number | null;
  /** @nullable */
  fuelConsumptionChange?: number | null;
  /** @nullable */
  fuelConsumptionPrevious?: number | null;
}
