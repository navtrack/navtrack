import { FormattedMessage } from "react-intl";

type ErrorMessageProps = {
  code: string;
};

export function ErrorMessage(props: ErrorMessageProps) {
  const id = `errors.${props.code}`;

  return <FormattedMessage id={id} />;
}
