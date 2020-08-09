import { PropertyValidationResult } from "./PropertyValidationResult";
import { ModelValidationResult } from "./ModelValidationResult";

export class ValidationResult<T> {
  property: ModelValidationResult<T> = {};

  AddError(key: keyof T, message: string): void {
    let propertyValidationResult = this.GetPropertyValidationResult(key);

    if (!propertyValidationResult) {
      propertyValidationResult = new PropertyValidationResult();
      this.property[key] = propertyValidationResult;
    }

    propertyValidationResult.AddError(message);
  }

  HasErrors(): boolean {
    for (const key in this.property) {
      const propertyValidationResult = this.property[key];

      if (propertyValidationResult && propertyValidationResult.HasErrors()) {
        return true;
      }
    }

    return false;
  }

  GetPropertyValidationResult(key: keyof T): PropertyValidationResult | undefined {
    return this.property[key];
  }

  ClearErrors() {
    this.property = {};
  }
}
