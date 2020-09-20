import { ValidationResult } from "../validation/ValidationResult";

export class ApiError<T> {
  validationResult?: ValidationResult<T>;
  message?: string;
  status: number;

  constructor(validationResult: ValidationResult<T>) {
    this.validationResult = validationResult;
    this.message = "";
    this.status = 0;
  }
}
