import { ValidationResult } from "components/Common/ValidatonResult";
export class AppError {
  validationResult: ValidationResult;
  message: string;
  status: number;

  constructor(errors: ValidationResult = {}) {
    this.validationResult = errors;
    this.message = "";
    this.status = 0;
  }
}
