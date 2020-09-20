import { useState } from "react";
import { useIntl } from "react-intl";
import { ApiError } from "../httpClient/AppError";
import { ValidationResult } from "./ValidationResult";
import { ValidateAction } from "./ValidateAction";

export function useNewValidation<T>(
  validator: ValidateAction<T>
): [(model: T) => boolean, ValidationResult<T>, (apiError: ApiError<T>) => void] {
  const intl = useIntl();
  const [validationResult, setValidationResult] = useState<ValidationResult<T>>(
    new ValidationResult<T>()
  );

  const validate = (model: T): boolean => {
    const validationResult = new ValidationResult<T>();

    validator(model, validationResult, intl);

    setValidationResult(validationResult);

    return !validationResult.HasErrors();
  };

  const setApiError = (apiError: ApiError<T>): void => {
    if (apiError.validationResult) {
      setValidationResult(apiError.validationResult);
    }
  };

  return [validate, validationResult, setApiError];
}
