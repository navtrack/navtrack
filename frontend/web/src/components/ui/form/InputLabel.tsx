import { FormattedMessage } from "react-intl";

type InputLabelProps = {
  label?: string;
  name?: string;
};

export function InputLabel(props: InputLabelProps) {
  if (!props.label) {
    return null;
  }

  return (
    <label
      htmlFor={props.name}
      className="mb-1 block text-sm font-medium leading-6 text-gray-900">
      <FormattedMessage id={props.label} />
    </label>
  );
}
