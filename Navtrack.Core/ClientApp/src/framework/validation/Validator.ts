import { ValidationResult } from "./ValidationResult";
import { IntlShape } from "react-intl";

export type Validator<T> = (
  object: T,
  validationResult: ValidationResult<T>,
  intl: IntlShape
) => void;
