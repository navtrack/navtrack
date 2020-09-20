import { ValidationResult } from "./ValidationResult";
import { IntlShape } from "react-intl";

export type ValidateAction<T> = (
  object: T,
  validationResult: ValidationResult<T>,
  intl: IntlShape
) => void;
