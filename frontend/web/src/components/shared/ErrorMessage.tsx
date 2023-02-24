import { isNumeric } from "@navtrack/ui-shared/utils/numbers";
import { FormattedMessage } from "react-intl";

type ErrorMessageProps = {
  code: string;
};

export const ErrorMessage = (props: ErrorMessageProps) => {
  const id = isNumeric(props.code) ? `errors.${props.code}` : props.code;

  return <FormattedMessage id={id} />;
};
