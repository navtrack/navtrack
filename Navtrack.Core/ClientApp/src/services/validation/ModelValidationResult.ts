import { PropertyValidationResult } from "./PropertyValidationResult";

export type ModelValidationResult<T> = Partial<Record<keyof T, PropertyValidationResult>>;
