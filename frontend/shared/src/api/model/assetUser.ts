/**
 * Generated by orval v6.31.0 🍺
 * Do not edit manually.
 * Navtrack.Api
 * OpenAPI spec version: 1.0.0
 */
import type { AssetUserRole } from "./assetUserRole";

export interface AssetUser {
  /** @minLength 1 */
  createdDate: string;
  /** @minLength 1 */
  email: string;
  /** @minLength 1 */
  userId: string;
  userRole: AssetUserRole;
}
