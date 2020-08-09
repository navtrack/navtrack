import { useState } from "react";
import { ValidationResult } from "./ValidationResult";
import { Validator } from "./Validator";
import { ApiError } from "framework/httpClient/AppError";
import { useIntl } from "react-intl";

export function  useNewValidation<T>(
  validator: Validator<T>
): [(model: T) => boolean, ValidationResult<T>, (apiError: ApiError<T>) => void] {
  const intl = useIntl();
  const [validationResult, setValidationResult] = useState<ValidationResult<T>>(new ValidationResult<T>());

  const validate = (model: T): boolean => {
    const validationResult = new ValidationResult<T>();

    validator(model, validationResult, intl);

    setValidationResult(validationResult);

    return !validationResult.HasErrors();
  };

  const setApiError = (apiError: ApiError<T>): void => {
    console.log(apiError);
    if (apiError.validationResult) {
      setValidationResult(apiError.validationResult);
    }
  };

  return [validate, validationResult, setApiError];
}
