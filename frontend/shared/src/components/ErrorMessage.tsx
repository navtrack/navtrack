import { FormattedMessage } from "react-intl";
import { isNumeric } from "../utils/numbers";

type ErrorMessageProps = {
  code: string;
};

export const ErrorMessage = (props: ErrorMessageProps) => {
  const id = isNumeric(props.code) ? `errors.${props.code}` : props.code;

  return <FormattedMessage id={id} />;
};
