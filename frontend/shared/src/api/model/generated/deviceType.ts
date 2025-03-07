/**
 * Generated by orval v6.31.0 🍺
 * Do not edit manually.
 * Navtrack.Api
 * OpenAPI spec version: 1.0.0
 */
import type { Protocol } from "./protocol";

export interface DeviceType {
  /** @minLength 1 */
  displayName: string;
  /** @minLength 1 */
  id: string;
  /** @minLength 1 */
  manufacturer: string;
  /** @minLength 1 */
  model: string;
  protocol: Protocol;
}
