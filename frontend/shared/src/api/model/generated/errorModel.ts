/**
 * Generated by orval v6.11.1 🍺
 * Do not edit manually.
 * Navtrack.Api, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
 * OpenAPI spec version: 1.0
 */
import type { ValidationErrorModel } from './validationErrorModel';

export interface ErrorModel {
  code?: string | null;
  message?: string | null;
  validationErrors?: ValidationErrorModel[] | null;
}